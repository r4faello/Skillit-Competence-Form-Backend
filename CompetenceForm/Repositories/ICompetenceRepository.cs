using CompetenceForm.Common;
using CompetenceForm.Models;

namespace CompetenceForm.Repositories
{
    public interface ICompetenceRepository
    {
        public Task<CompetenceSet?> GetCurrentCompetenceSet();
        public Task<CompetenceSet?> GetCompetenceSetById(string competenceSetId);
        public Task<CompetenceSet?> GetCompetenceSetByIdInclusive(string competenceSetId);
        public Task<Result> RegisterAnsweredQuestion(Draft draft, Question question, Answer answer);
        public Task<Draft> CreateNewDraft(User author, CompetenceSet competenceSet);
        public Task<Draft?> GetDraftByIdInclusive(string draftId);


    }
}
