using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<(Result, CompetenceSetDto?)> SpitCompetenceSet(User user);
        public Task<Result> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId);


        public Task<Result> Seed(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange);


        public Task<string> GetCurrentCompetenceSetId();
        public Task<Result> DeleteUserDrafts(User user);
    }
}
