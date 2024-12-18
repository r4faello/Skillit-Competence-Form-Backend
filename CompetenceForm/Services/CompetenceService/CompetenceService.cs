using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Migrations;
using CompetenceForm.Models;
using CompetenceForm.Repositories;
using CompetenceForm.Repositories._Queries;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Services.CompetenceService
{
    public class CompetenceService : ICompetenceService
    {
        private readonly ICompetenceRepository _competenceRepository;
        private readonly IUserRepository _userRepository;

        public CompetenceService(ICompetenceRepository competenceRepository, IUserRepository userRepository)
        {
            _competenceRepository = competenceRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId)
        {
            // Check if given competence set ID is valid
            var competenceSetQuery = new CompetenceSetQuery {
                IncludeCompetences = true,
                IncludeCompetenceQuestion = true,
                IncludeCompetenceQuestionAnswerOptions = true
            };

            var competenceSet = await _competenceRepository.GetCompetenceSetByIdAsync(competenceSetId, competenceSetQuery);
            if(competenceSet == null){return Result.Failure("Competence set not found");}

            // Check if given competence ID and question object itself is valid
            var competence = competenceSet.Competences.FirstOrDefault(c => c.Id == competenceId);
            if (competence == null){return Result.Failure("Competence not found");}
            if(competence.Question == null){return Result.Failure("Answered question is not valid.");}

            // Check if answerId is among options for current question
            var answer = competence.Question.AnswerOptions.FirstOrDefault(a => a.Id == answerId);
            if (answer == null){return Result.Failure("Answer option not found");}



            // Retrieve the user's draft linked to the competence set ID; create a new draft if it doesn't exist
            var draft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == competenceSetId);
            if(draft == null)
            {
                draft = await _competenceRepository.CreateNewDraftAsync(user, competenceSet);
            }

            var draftQuery = new DraftQuery {
                IncludeQuestionAnswerPairs = true,
                IncludeQuestionAnswerPairQuestion = true,
                IncludeQuestionAnswerPairAnswer = true,
                IncludeAuthor = true
            };
            draft = await _competenceRepository.GetDraftByIdAsync(draft.Id, draftQuery);

            await _competenceRepository.RegisterAnsweredQuestionAsync(draft, competence.Question, answer);


            return Result.Success();
        }


        public async Task<(Result, CompetenceSetDto?)> SpitCompetenceSet(User user)
        {
            var currentCompetenceSet = await _competenceRepository.GetCurrentCompetenceSetAsync();

            if (currentCompetenceSet == null)
            {
                return (Result.Failure("Competence set not found"), null);
            }
            else if(currentCompetenceSet.Competences == null || currentCompetenceSet.Competences.Count == 0)
            {
                return (Result.Failure("No competences found in specified competence set"), null);
            }

            var currentCompSetDraft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == currentCompetenceSet.Id);
            if(currentCompSetDraft != null)
            {
                DraftQuery draftQuery = new DraftQuery
                {
                    IncludeQuestionAnswerPairs = true,
                    IncludeQuestionAnswerPairQuestion = true,
                    IncludeQuestionAnswerPairAnswer = true
                };

                currentCompSetDraft = await _competenceRepository.GetDraftByIdAsync(currentCompSetDraft.Id, draftQuery);
            }
            
            
            var response = new CompetenceSetDto
            {
                CompetenceSetId = currentCompetenceSet.Id,
                Competences = currentCompetenceSet.Competences
                    .Where(c => c.Question != null)
                    .Select(c => new CompetenceDto
                    {
                        CompetenceId = c.Id,
                        Question = c.Question.Title,
                        AnswerOptions = c.Question.AnswerOptions.Select(a => new AnswerOptionDto
                        {
                            AnswerId = a.Id,
                            Answer = a.Title,
                            Description = a.Description
                        }).ToList(),
                        DraftedAnswerId = currentCompSetDraft == null ? "" :
                            currentCompSetDraft.QuestionAnswerPairs.Any(a => a.Question.Id == c.Question.Id) ?
                            currentCompSetDraft.QuestionAnswerPairs.First(a => a.Question.Id == c.Question.Id).Answer.Id : ""
                    }).ToList()
            };

            return (Result.Success(), response);
        }

        public async Task<Result> Seed(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange)
        {
            var wipeResult = await _competenceRepository.WipeCompetenceSets();
            if (!wipeResult.IsSuccess)
            {
                return Result.Failure("Deletion failed.");
            }


            var (compSetResult, competenceSet) = await _competenceRepository.CreateRandomCompetenceSetAsync(competenceCount, answerCountRange, answerImpactRange);
            if (!compSetResult.IsSuccess)
            {
                return Result.Failure("Internal error.");
            }

            return Result.Success();
        }

        public async Task<string> GetCurrentCompetenceSetId()
        {
            var currentSet = await _competenceRepository.GetCurrentCompetenceSetAsync();
            if(currentSet == null)
            {
                return "";
            }

            return currentSet.Id;
        }

        public Task<Result> DeleteUserDrafts(User user)
        {
            var result = _competenceRepository.DeleteUserDrafts(user);

            return result;
        }

        public async Task<Result> FinalizeDraft(User user)
        {
            if(user == null) { return Result.Failure("User not valid"); }

            var currentCompetenceSet = await _competenceRepository.GetCurrentCompetenceSetAsync();
            if(currentCompetenceSet == null){ return Result.Failure("Current competence set was not found."); }

            
            var (result, submittedRecord) = await _competenceRepository.FinalizeDraftAsync(user, currentCompetenceSet.Id);
            if (!result.IsSuccess) { return result; }
            
            var deletionResult = await _competenceRepository.DeleteUserDrafts(user);
            if (!deletionResult.IsSuccess) { return result; }


            return Result.Success();
        }

        public async Task<(Result, List<SubmittedRecordDto>?)> SpitSubmittedRecords()
        {
            var (result, submittedRecords) = await _competenceRepository.GetAllSubmittedRecords();

            if (submittedRecords == null || !submittedRecords.Any())
            {
                return (Result.Failure("No submitted records found"), null);
            }

            
            var response = submittedRecords.Select(submittedRecord => new SubmittedRecordDto
            {
                RecordId = submittedRecord.Id,
                AuthorId = submittedRecord.AuthorId,
                AuthorUsername = submittedRecord.Author?.Username ?? "Unknown User",
                Competences = submittedRecord.CompetenceValues.Select(cv => new CompetenceValueDto
                {
                    CompetenceId = cv.Competence?.Id ?? Guid.Empty.ToString(),
                    CompetenceTitle = cv.Competence?.Title ?? "Unknown Skill",
                    Value = cv.Value
                }).ToList(),
                SubmittedAt = submittedRecord.SubmittedAt
            }).ToList();

            return (Result.Success(), response);
        }

        public async Task<(Result, int?)> GetUnfinishedUserCount()
        {
            var (result, count) = await _competenceRepository.GetUnfinishedUserCount();

            if (!result.IsSuccess)
            {
                return (result, null);
            }

            return (result, count);
        }

        public async Task<Result> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData)
        {
            var (result, competenceSet) = await _competenceRepository.CreateCompetenceSetFromJsonAsync(jsonData);

            if (!result.IsSuccess)
            {
                return Result.Failure("Failed to seed competence set.");
            }

            return Result.Success();
        }


    }
}
