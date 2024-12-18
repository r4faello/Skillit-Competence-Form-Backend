using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using LoremNET;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                var existingAnswer = draft.QuestionAnswerPairs.FirstOrDefault(qa => qa.Question.Equals(question));
                if (existingAnswer != null)
                {
                    existingAnswer.Answer = answer;
                }
                else
                {
                    draft.QuestionAnswerPairs.Add(new QuestionAnswer(question, answer));
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
                draftQuery = draftQuery.Include(d => d.QuestionAnswerPairs);

                if (query.IncludeQuestionAnswerPairQuestion)
                {
                    draftQuery = draftQuery.Include(d => d.QuestionAnswerPairs)
                                           .ThenInclude(a => a.Question);
                }

                if (query.IncludeQuestionAnswerPairAnswer)
                {
                    draftQuery = draftQuery.Include(d => d.QuestionAnswerPairs)
                                           .ThenInclude(a => a.Answer);
                }
            }

            return await draftQuery.FirstOrDefaultAsync(d => d.Id == draftId);
        }




        public async Task<Result> WipeCompetenceSets()
        {
            try
            {
                await _context.CompetenceValues.ExecuteDeleteAsync();
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

        public async Task<Result> DeleteUserDrafts(User user)
        {
            try
            {
                var answersToRemove = _context.Drafts
                    .Where(d => d.Author == user)
                    .SelectMany(d => d.QuestionAnswerPairs)
                    .ToList();
                _context.QuestionAnswerPairs.RemoveRange(answersToRemove);

                _context.Drafts.RemoveRange(_context.Drafts.Where(d => d.Author == user));
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.Failure("Internal error.");
            }
        }

        public async Task<(Result, SubmittedRecord?)> FinalizeDraftAsync(User user, string competenceSetId)
        {
            var draft = await _context.Drafts
                .Include(d=> d.QuestionAnswerPairs)
                .ThenInclude(qap => qap.Question)
                .Include(d=> d.QuestionAnswerPairs)
                .ThenInclude(qap => qap.Answer)
                .FirstOrDefaultAsync(d => d.Author.Id == user.Id);
            if(draft == null){ return (Result.Failure("Draft not found"), null); }

            if(draft?.QuestionAnswerPairs == null || !draft.QuestionAnswerPairs.Any()){ return (Result.Failure("Draft is empty"), null);}
            

            var competenceValues = new List<CompetenceValue>();
            foreach (var competence in draft.CompetenceSet.Competences)
            {
                if(competence == null || competence.Question == null) { continue; }

                var pair = draft. QuestionAnswerPairs.FirstOrDefault(qap => qap.Question.Id == competence.Question.Id);

                int? numericResult = pair != null ? pair.Answer.InpactOnCompetence : null;
                
                var newCompValue = new CompetenceValue(competence, numericResult);

                competenceValues.Add(newCompValue);
            }

            
            var newSubmittedRecord = new SubmittedRecord(user, competenceSetId, competenceValues);
            await _context.SubmittedRecords.AddAsync(newSubmittedRecord);
            await _context.SaveChangesAsync();

            return (Result.Success(), newSubmittedRecord);
        }
        public bool HasQuestionAnswerPairs(Draft draft)
        {
            return draft?.QuestionAnswerPairs != null && draft.QuestionAnswerPairs.Any();
        }

        public async Task<(Result, List<SubmittedRecord>?)> GetAllSubmittedRecords()
        {
            try
            {
                var submittedRecords = await _context.SubmittedRecords.Include(sr=>sr.Author).Include(sr => sr.CompetenceValues).ThenInclude(cv => cv.Competence).ToListAsync();
                return (Result.Success(), submittedRecords);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return (Result.Failure("Internal error."), null);
            }
        }

        public async Task<(Result, int?)> GetUnfinishedUserCount()
        {
            try
            {
                var userCount = await _context.Drafts
                    .Select(d => d.Author.Id)
                    .Distinct()
                    .CountAsync();

                return (Result.Success(), userCount);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return (Result.Failure("Internal error."), null);
            }
        }

        public async Task<(Result, CompetenceSet?)> CreateCompetenceSetFromJsonAsync(CompetenceSetJson jsonData)
        {
            try
            {
                Console.WriteLine($"Incoming JSON Data: {jsonData}");

                // Configure the serializer to accept camelCase JSON
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var competenceSetData = jsonData;

                if (competenceSetData == null || competenceSetData.Skills == null || !competenceSetData.Skills.Any())
                {
                    return (Result.Failure("Invalid or empty JSON data."), null);
                }

                // Create a new CompetenceSet
                var competenceSet = new CompetenceSet();

                foreach (var skill in competenceSetData.Skills)
                {
                    foreach (var questionData in skill.Questions)
                    {
                        // Create answers
                        var answers = questionData.AnswerOptions.Select(option => new Answer
                        {
                            Title = option.Option,
                            Description = option.Description,
                            InpactOnCompetence = option.Points
                        }).ToList();

                        // Create question
                        var question = new Question
                        {
                            Title = questionData.Question,
                            AnswerOptions = answers
                        };

                        // Add question and answers to the database
                        await _context.Questions.AddAsync(question);

                        // Create competence
                        var competence = new Competence
                        {
                            Title = skill.SkillName,
                            Question = question
                        };

                        // Add competence to the set
                        competenceSet.Competences.Add(competence);

                        // Add competence to the database
                        await _context.Competences.AddAsync(competence);
                    }
                }

                // Save the CompetenceSet
                await _context.CompetenceSets.AddAsync(competenceSet);
                await _context.SaveChangesAsync();

                return (Result.Success(), competenceSet);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return (Result.Failure("Failed to create competence set from JSON."), null);
            }
        }



        // Helper class to deserialize JSON
        public class CompetenceSetJson
        {
            public List<SkillJson> Skills { get; set; } = new List<SkillJson>();
        }

        public class SkillJson
        {
            public string SkillName { get; set; } = "";
            public List<QuestionJson> Questions { get; set; } = new List<QuestionJson>();
        }

        public class QuestionJson
        {
            public string Question { get; set; } = "";
            public List<AnswerOptionJson> AnswerOptions { get; set; } = new List<AnswerOptionJson>();
        }

        public class AnswerOptionJson
        {
            public string Option { get; set; } = "";
            public int Points { get; set; }
            public string Description { get; set; }
        }


    }
}
