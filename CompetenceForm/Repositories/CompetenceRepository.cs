using CompetenceForm.Common;
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

        public async Task<CompetenceSet?> GetCurrentCompetenceSet()
        {
            var currentCompetence = await _context.CompetenceSets
                .Include(cs => cs.Competences)
                .ThenInclude(c => c.Question)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync();

            return currentCompetence;
        }


        public async Task<CompetenceSet?> GetCompetenceSetById(string competenceSetId)
        {
            var competenceSet = await _context.CompetenceSets.FirstOrDefaultAsync(cs => cs.Id == competenceSetId);
            
            return competenceSet;
        }

        public async Task<CompetenceSet?> GetCompetenceSetByIdInclusive(string competenceSetId)
        {
            var competenceSet = await _context.CompetenceSets
                .Include(cs => cs.Competences)
                .ThenInclude(c => c.Question)
                .ThenInclude(c => c.AnswerOptions)
                .FirstOrDefaultAsync(cs => cs.Id == competenceSetId);

            return competenceSet;
        }



        public async Task<Result> RegisterAnsweredQuestion(Draft draft, Question question, Answer answer)
        {
            try
            {
                var existingAnswer = draft.Answers.FirstOrDefault(qa => qa.Question.Equals(question));
                if (existingAnswer != null)
                {
                    existingAnswer.Answer = answer;
                }
                else
                {
                    draft.Answers.Add(new QuestionAnswer(question, answer));
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return Result.Failure("Internal error.");
            }
            

            return Result.Success();
        }

        public async Task<Draft> CreateNewDraft(User author, CompetenceSet competenceSet)
        {
            var newDraft = new Draft(author, competenceSet);
            author.Drafts.Add(newDraft);
            await _context.SaveChangesAsync();

            return newDraft;
        }

        public async Task<Draft?> GetDraftByIdInclusive(string draftId)
        {
            return await _context.Drafts
                .Include(d => d.Author)
                .Include(d => d.Answers)
                    .ThenInclude(a => a.Question)
                .Include(d => d.Answers)
                    .ThenInclude(a => a.Answer)
                .FirstOrDefaultAsync(d => d.Id == draftId);
        }
    }
}
