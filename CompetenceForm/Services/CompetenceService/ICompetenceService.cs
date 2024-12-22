using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<ServiceResult<CompetenceSetDto>> SpitCompetenceSet(User user);
        public Task<ServiceResult> SaveAnsweredQuestion(User user, string competenceSetId, string competenceId, string answerId);
        public Task<ServiceResult> Seed(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange);
        public Task<string> GetCurrentCompetenceSetId();
        public Task<ServiceResult> DeleteUserDrafts(User user);
        public Task<ServiceResult<SubmittedRecordDto>> FinalizeDraft(User user);
        public Task<ServiceResult<List<SubmittedRecordDto>>> SpitSubmittedRecords();
        public Task<ServiceResult<int>> GetUnfinishedUserCount();
        public Task<ServiceResult> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData);
    }
}
