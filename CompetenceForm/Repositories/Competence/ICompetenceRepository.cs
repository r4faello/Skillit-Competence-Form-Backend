using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Repositories
{
    public interface ICompetenceRepository
    {
        public Task<ServiceResult<CompetenceSet>> GetCurrentCompetenceSetAsync();
        public Task<ServiceResult<CompetenceSet>> GetCompetenceSetByIdAsync(string competenceSetId, CompetenceSetQuery competenceSetQuery);
        public Task<ServiceResult> RegisterAnsweredQuestionAsync(Draft? draft, Question question, Answer answer);
        public Task<ServiceResult<Draft>> CreateNewDraftAsync(User author, CompetenceSet competenceSet);
        public Task<ServiceResult<Draft>> GetDraftByIdAsync(string draftId, DraftQuery query);
        public Task<ServiceResult> WipeCompetenceSetsAsync();
        public Task<ServiceResult<Competence>> CreateRandomCompetenceAsync((int, int) answerCountRange, (int, int) answerImpactRange);
        public Task<ServiceResult<CompetenceSet>> CreateRandomCompetenceSetAsync(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange);
        public Task<ServiceResult> DeleteUserDraftsAsync(User user);
        public Task<ServiceResult<SubmittedRecord>> FinalizeDraftAsync(User user, string competenceSetId);
        public Task<ServiceResult<List<SubmittedRecord>>> GetAllSubmittedRecordsAsync();
        public Task<ServiceResult<int>> GetUnfinishedUserCountAsync();
        public Task<ServiceResult<CompetenceSet>> CreateCompetenceSetFromJsonAsync(CompetenceSetJson jsonData);
    }
}
