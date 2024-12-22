using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<ServiceResult<CompetenceSetDto>> GetCompetenceSetAsync(User user);
        public Task<ServiceResult> SaveAnsweredQuestionAsync(User user, string competenceSetId, string competenceId, string answerId);
        public Task<ServiceResult> SeedCompetencesAsync(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange);
        public Task<string> GetCurrentCompetenceSetIdAsync();
        public Task<ServiceResult> DeleteUserDraftsAsync(User user);
        public Task<ServiceResult<SubmittedRecordDto>> FinalizeDraftAsync(User user);
        public Task<ServiceResult<List<SubmittedRecordDto>>> GetSubmittedRecordsAsync();
        public Task<ServiceResult<int>> GetUnfinishedUserCountAsync();
        public Task<ServiceResult> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData);
    }
}
