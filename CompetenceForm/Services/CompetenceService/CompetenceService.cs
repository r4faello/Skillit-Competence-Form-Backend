using AutoMapper;
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

        public async Task<ServiceResult> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId)
        {
            // Check if given competence set ID is valid
            var competenceSetQuery = new CompetenceSetQuery
            {
                IncludeCompetences = true,
                IncludeCompetenceQuestion = true,
                IncludeCompetenceQuestionAnswerOptions = true
            };

            var result = await _competenceRepository.GetCompetenceSetByIdAsync(competenceSetId, competenceSetQuery);
            if (!result.IsSuccess || result.Data == null) { return result; }

            // Check if given competence ID and question object itself is valid
            var competence = result.Data.Competences.FirstOrDefault(c => c.Id == competenceId);
            if (competence == null) { return ServiceResult.Failure("Competence not found"); }
            if (competence.Question == null) { return ServiceResult.Failure("Answered question is not valid."); }

            // Check if answerId is among options for current question
            var answer = competence.Question.AnswerOptions.FirstOrDefault(a => a.Id == answerId);
            if (answer == null) { return ServiceResult.Failure("Answer option not found"); }

            // Retrieve the user's draft linked to the competence set ID; create a new draft if it doesn't exist
            var draft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == competenceSetId);
            if (draft == null)
            {
                var draftCreationResult = await _competenceRepository.CreateNewDraftAsync(user, result.Data);
                if (!draftCreationResult.IsSuccess || draftCreationResult.Data == null) { return draftCreationResult; }

                draft = draftCreationResult.Data;
            }

            var draftQuery = new DraftQuery
            {
                IncludeQuestionAnswerPairs = true,
                IncludeQuestionAnswerPairQuestion = true,
                IncludeQuestionAnswerPairAnswer = true,
                IncludeAuthor = true
            };

            var getDraftResult = await _competenceRepository.GetDraftByIdAsync(draft.Id, draftQuery);
            if (!getDraftResult.IsSuccess || getDraftResult.Data == null) { return getDraftResult; }

            draft = getDraftResult.Data;

            await _competenceRepository.RegisterAnsweredQuestionAsync(draft, competence.Question, answer);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<CompetenceSetDto>> SpitCompetenceSet(User user)
        {
            var currentCompetenceSetResult = await _competenceRepository.GetCurrentCompetenceSetAsync();

            if (!currentCompetenceSetResult.IsSuccess || currentCompetenceSetResult.Data == null)
            {
                return ServiceResult<CompetenceSetDto>.Failure("Competence set not found");
            }
            else if(currentCompetenceSetResult.Data.Competences == null || currentCompetenceSetResult.Data.Competences.Count == 0)
            {
                return ServiceResult<CompetenceSetDto>.Failure("No competences found in specified competence set");
            }

            var currentCompSetDraft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == currentCompetenceSetResult.Data.Id);
            if(currentCompSetDraft != null)
            {
                DraftQuery draftQuery = new DraftQuery
                {
                    IncludeQuestionAnswerPairs = true,
                    IncludeQuestionAnswerPairQuestion = true,
                    IncludeQuestionAnswerPairAnswer = true
                };

                var getDraftResult = await _competenceRepository.GetDraftByIdAsync(currentCompSetDraft.Id, draftQuery);
                if(!getDraftResult.IsSuccess || getDraftResult.Data == null) { return ServiceResult<CompetenceSetDto>.Failure(getDraftResult.Message); }

                currentCompSetDraft = getDraftResult.Data;
            }

            var currentCompetenceSet = currentCompetenceSetResult.Data;
            
            
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

            return ServiceResult<CompetenceSetDto>.Success(response);
        }

        public async Task<ServiceResult> Seed(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange)
        {
            var wipeResult = await _competenceRepository.WipeCompetenceSetsAsync();
            if (!wipeResult.IsSuccess)
            {
                return ServiceResult.Failure("Deletion failed.");
            }


            var createRandomCompSetResult = await _competenceRepository.CreateRandomCompetenceSetAsync(competenceCount, answerCountRange, answerImpactRange);
            if (!createRandomCompSetResult.IsSuccess)
            {
                return ServiceResult.Failure("Internal error.");
            }

            return ServiceResult.Success();
        }

        public async Task<string> GetCurrentCompetenceSetId()
        {
            var result = await _competenceRepository.GetCurrentCompetenceSetAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                return "";
            }

            return result.Data.Id;
        }

        public Task<ServiceResult> DeleteUserDrafts(User user)
        {
            var result = _competenceRepository.DeleteUserDraftsAsync(user);

            return result;
        }

        public async Task<ServiceResult<SubmittedRecordDto>> FinalizeDraft(User user)
        {
            if (user == null) { return ServiceResult<SubmittedRecordDto>.Failure("User not valid"); }

            var currentCompetenceSetResult = await _competenceRepository.GetCurrentCompetenceSetAsync();
            if (!currentCompetenceSetResult.IsSuccess || currentCompetenceSetResult.Data == null) { return ServiceResult<SubmittedRecordDto>.Failure("Current competence set was not found."); }

            var finalizeDraftResult = await _competenceRepository.FinalizeDraftAsync(user, currentCompetenceSetResult.Data.Id);
            if (!finalizeDraftResult.IsSuccess || finalizeDraftResult.Data == null) { return ServiceResult<SubmittedRecordDto>.Failure(finalizeDraftResult.Message); }
            var submittedRecord = finalizeDraftResult.Data;


            var deletionResult = await _competenceRepository.DeleteUserDraftsAsync(user);
            if (!deletionResult.IsSuccess) { return ServiceResult<SubmittedRecordDto>.Failure(deletionResult.Message); }

            // Create SubmittedRecordDto from submittedRecord
            var submittedRecordDto = new SubmittedRecordDto
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
            };

            return ServiceResult<SubmittedRecordDto>.Success(submittedRecordDto);
        }

        public async Task<ServiceResult<List<SubmittedRecordDto>>> SpitSubmittedRecords()
        {
            var submittedRecordsResult = await _competenceRepository.GetAllSubmittedRecordsAsync();

            if (!submittedRecordsResult.IsSuccess || submittedRecordsResult.Data == null || !submittedRecordsResult.Data.Any())
            {
                return ServiceResult<List<SubmittedRecordDto>>.Failure("No submitted records found");
            }

            var response = submittedRecordsResult.Data.Select(submittedRecord => new SubmittedRecordDto
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

            return ServiceResult<List<SubmittedRecordDto>>.Success(response);
        }

        public async Task<ServiceResult<int>> GetUnfinishedUserCount()
        {
            var result = await _competenceRepository.GetUnfinishedUserCountAsync();

            if (!result.IsSuccess)
            {
                return ServiceResult<int>.Failure(result.Message);
            }

            return ServiceResult<int>.Success(result.Data);
        }

        public async Task<ServiceResult> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData)
        {
            var wipeResult = await _competenceRepository.WipeCompetenceSetsAsync();

            if (!wipeResult.IsSuccess)
            {
                return wipeResult;
            }


            var createCompetenceSetResult = await _competenceRepository.CreateCompetenceSetFromJsonAsync(jsonData);

            if (!createCompetenceSetResult.IsSuccess)
            {
                return ServiceResult.Failure("Failed to seed competence set.");
            }

            return ServiceResult.Success();
        }
    }
}
