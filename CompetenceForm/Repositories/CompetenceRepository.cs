using CompetenceForm.Models;
using Microsoft.EntityFrameworkCore;

namespace CompetenceForm.Repositories
{
    public class CompetenceRepository : ICompetenceRepository
    {
        private readonly ApplicationDbContext _context;

        public CompetenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<Competence>? GetCompetences(CompetenceSet competenceSet)
        {
            var currentCompetenceSet = competenceSet;
            if (currentCompetenceSet == null)
            {
                return null;
            }

            return currentCompetenceSet.Competences.ToList();
        }

        public async Task<CompetenceSet?> GetCurrentCompetenceSet()
        {
            var currentCompetence = await _context.CompetenceSets
                .Include(cs => cs.Competences)
                .ThenInclude(c => c.Question)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync();

            return currentCompetence;
        }
    }
}
