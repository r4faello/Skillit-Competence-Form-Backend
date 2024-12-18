using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<(Result, CompetenceSetDto?)> SpitCompetenceSet(User user);
        public Task<Result> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId);
        public Task<Result> Seed(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange);
        public Task<string> GetCurrentCompetenceSetId();
        public Task<Result> DeleteUserDrafts(User user);
        public Task<Result> FinalizeDraft(User user);
        public Task<(Result, List<SubmittedRecordDto>?)> SpitSubmittedRecords();
        public Task<(Result, int?)> GetUnfinishedUserCount();
        public Task<Result> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData);
    }
}
