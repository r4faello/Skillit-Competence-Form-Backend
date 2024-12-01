using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;

namespace CompetenceForm.Repositories
{
    public interface ICompetenceRepository
    {
        public Task<CompetenceSet?> GetCurrentCompetenceSetAsync();
        public Task<CompetenceSet?> GetCompetenceSetByIdAsync(string competenceSetId, CompetenceSetQuery competenceSetQuery);
        public Task<Result> RegisterAnsweredQuestionAsync(Draft? draft, Question question, Answer answer);
        public Task<Draft> CreateNewDraftAsync(User author, CompetenceSet competenceSet);
        public Task<Draft?> GetDraftByIdAsync(string draftId, DraftQuery query);
    }
}
