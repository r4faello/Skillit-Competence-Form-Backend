using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories;

namespace CompetenceForm.Services.CompetenceService
{
    public class CompetenceService : ICompetenceService
    {
        private readonly ICompetenceRepository _competenceRepository;

        public CompetenceService(ICompetenceRepository competenceRepository)
        {
            _competenceRepository = competenceRepository;
        }

        public async Task<(Result, CompetenceSetDto?)> SpitCompetenceSet()
        {
            var currentCompetenceSet = await _competenceRepository.GetCurrentCompetenceSet();

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
                Competences = currentCompetenceSet.Competences.Select(c => new CompetenceDto
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
