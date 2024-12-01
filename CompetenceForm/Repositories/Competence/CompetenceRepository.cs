using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
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

        public async Task<CompetenceSet?> GetCurrentCompetenceSetAsync()
        {
            var currentCompetence = await _context.CompetenceSets
                .Include(cs => cs.Competences)
                .ThenInclude(c => c.Question)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync();

            return currentCompetence;
        }

        public async Task<CompetenceSet?> GetCompetenceSetByIdAsync(string competenceSetId, CompetenceSetQuery query)
        {
            var competenceSetQuery = _context.CompetenceSets.AsQueryable();

            if (query.IncludeCompetences)
            {
                competenceSetQuery = competenceSetQuery.Include(cs => cs.Competences);

                if (query.IncludeCompetenceQuestion)
                {
                    competenceSetQuery = competenceSetQuery.Include(cs => cs.Competences)
                                                           .ThenInclude(c => c.Question);

                    if (query.IncludeCompetenceQuestionAnswerOptions)
                    {
                        competenceSetQuery = competenceSetQuery.Include(cs => cs.Competences)
                                                               .ThenInclude(c => c.Question)
                                                               .ThenInclude(q => q.AnswerOptions);
                    }
                }
            }

            return await competenceSetQuery.FirstOrDefaultAsync(cs => cs.Id == competenceSetId);
        }




        public async Task<Result> RegisterAnsweredQuestionAsync(Draft? draft, Question question, Answer answer)
        {
            if(draft == null)
            {
                return Result.Failure("Draft not valid.");
            }

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

        public async Task<Draft> CreateNewDraftAsync(User author, CompetenceSet competenceSet)
        {
            var newDraft = new Draft(author, competenceSet);
            author.Drafts.Add(newDraft);
            await _context.SaveChangesAsync();

            return newDraft;
        }

        public async Task<Draft?> GetDraftByIdAsync(string draftId, DraftQuery query)
        {
            var draftQuery = _context.Drafts.AsQueryable();

            if (query.IncludeAuthor)
            {
                draftQuery = draftQuery.Include(d => d.Author);
            }

            if (query.IncludeCompetenceSet)
            {
                draftQuery = draftQuery.Include(d => d.CompetenceSet);
            }

            if (query.IncludeQuestionAnswerPairs)
            {
                draftQuery = draftQuery.Include(d => d.Answers);

                if (query.IncludeQuestionAnswerPairQuestion)
                {
                    draftQuery = draftQuery.Include(d => d.Answers)
                                           .ThenInclude(a => a.Question);
                }

                if (query.IncludeQuestionAnswerPairAnswer)
                {
                    draftQuery = draftQuery.Include(d => d.Answers)
                                           .ThenInclude(a => a.Answer);
                }
            }

            return await draftQuery.FirstOrDefaultAsync(d => d.Id == draftId);
        }
    }
}
