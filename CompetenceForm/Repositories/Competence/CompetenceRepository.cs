using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using LoremNET;
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




        public async Task<Result> WipeCompetenceSets()
        {
            try
            {
                
                await _context.Competences.ExecuteDeleteAsync();
                await _context.QuestionAnswerPairs.ExecuteDeleteAsync();
                await _context.Answers.ExecuteDeleteAsync();
                await _context.Questions.ExecuteDeleteAsync();
                await _context.CompetenceSets.ExecuteDeleteAsync();
                await _context.Drafts.ExecuteDeleteAsync();

                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return Result.Failure("Internal error.");
            }
        }

        public async Task<(Result, Competence?)> CreateRandomCompetenceAsync((int, int) answerCountRange, (int, int) answerImpactRange)
        {
            var answerOptions = new List<Answer>();
            var rand = new Random();
            var answerCount = (int)rand.NextInt64(answerCountRange.Item1, answerCountRange.Item2);

            try
            {
                for (int i=0; i<answerCount; i++)
                {
                    var ans = new Answer(Lorem.Words(1, 3), (int)rand.NextInt64(answerImpactRange.Item1, answerImpactRange.Item2), Lorem.Sentence(5, 15));
                    await _context.Answers.AddAsync(ans);
                    answerOptions.Add(ans);
                }

                var question = new Question(Lorem.Words(3,6) + "?", answerOptions);
                await _context.Questions.AddAsync(question);

                var competence = new Competence(question, "Skill " + Lorem.Words(1, false, false));
                await _context.Competences.AddAsync(competence);


                await _context.SaveChangesAsync();
                return (Result.Success(), competence);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return (Result.Failure("Failed to save competence."), null);
            }
        }

        public async Task<(Result, CompetenceSet?)> CreateRandomCompetenceSetAsync(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange)
        {
            try
            {
                var competences = new List<Competence>();

                for(int i=0; i<competenceCount; i++)
                {
                    var (competenceCreationResult, competence) = await CreateRandomCompetenceAsync(answerCountRange, answerImpactRange);
                    if (!competenceCreationResult.IsSuccess || competence == null)
                    {
                        await Console.Out.WriteLineAsync("Competence failed to create: " + competenceCreationResult.Message);
                    }
                    else
                    {
                        competences.Add(competence);
                    }
                }

                var competenceSet = new CompetenceSet(competences);
                _context.CompetenceSets.Add(competenceSet);

                await _context.SaveChangesAsync();
                return (Result.Success(), competenceSet);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return (Result.Failure("Competence set failed to save."), null);
            }
        }
    }
}
