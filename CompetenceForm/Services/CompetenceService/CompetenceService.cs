using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories;
using CompetenceForm.Repositories._Queries;

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

        public async Task<(Result, CompetenceSetDto?)> SpitCompetenceSet()
        {
            var currentCompetenceSet = await _competenceRepository.GetCurrentCompetenceSetAsync();

            if(currentCompetenceSet == null)
            {
                return (Result.Failure("Competence set not found"), null);
            }
            else if(currentCompetenceSet.Competences == null || currentCompetenceSet.Competences.Count == 0)
            {
                return (Result.Failure("No competences found in specified competence set"), null);
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
                            Answer = a.Title
                        }).ToList()
                    }).ToList()
            };

            return (Result.Success(), response);
        }
    }
}
