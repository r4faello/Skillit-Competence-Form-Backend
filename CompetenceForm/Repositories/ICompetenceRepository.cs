using CompetenceForm.Models;

namespace CompetenceForm.Repositories
{
    public interface ICompetenceRepository
    {
        public Task<CompetenceSet?> GetCurrentCompetenceSet();
        public List<Competence>? GetCompetences(CompetenceSet competenceSet);
    }
}
