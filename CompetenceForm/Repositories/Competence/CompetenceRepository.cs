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

        public async Task<ServiceResult<CompetenceSet>> GetCurrentCompetenceSetAsync()
        {
            var currentCompetence = await _context.CompetenceSets
                .Include(cs => cs.Competences)
                .ThenInclude(c => c.Question)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync();

            if (currentCompetence == null)
            {
                return ServiceResult<CompetenceSet>.Failure("No current competence set found.");
            }

            return ServiceResult<CompetenceSet>.Success(currentCompetence);
        }

        public async Task<ServiceResult<CompetenceSet>> GetCompetenceSetByIdAsync(string competenceSetId, CompetenceSetQuery query)
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

            var competenceSet = await competenceSetQuery.FirstOrDefaultAsync(cs => cs.Id == competenceSetId);

            if (competenceSet == null)
            {
                return ServiceResult<CompetenceSet>.Failure("Competence set not found.");
            }

            return ServiceResult<CompetenceSet>.Success(competenceSet);
        }

        public async Task<ServiceResult> RegisterAnsweredQuestionAsync(Draft? draft, Question question, Answer answer)
        {
            if(draft == null)
            {
                return ServiceResult.Failure("Draft not valid.");
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
                return ServiceResult.Failure("Internal error.");
            }


            return ServiceResult.Success();
        }

        public async Task<ServiceResult<Draft>> CreateNewDraftAsync(User author, CompetenceSet competenceSet)
        {
            var newDraft = new Draft(author, competenceSet);
            author.Drafts.Add(newDraft);

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult<Draft>.Success(newDraft);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<Draft>.Failure("Failed to create new draft.");
            }
        }

        public async Task<ServiceResult<Draft>> GetDraftByIdAsync(string draftId, DraftQuery query)
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

            var draft = await draftQuery.FirstOrDefaultAsync(d => d.Id == draftId);

            if (draft == null)
            {
                return ServiceResult<Draft>.Failure("Draft not found.");
            }

            return ServiceResult<Draft>.Success(draft);
        }

        public async Task<ServiceResult> WipeCompetenceSetsAsync()
        {
            try
            {
                await _context.CompetenceValues.ExecuteDeleteAsync();
                await _context.SubmittedRecords.ExecuteDeleteAsync();
                await _context.Competences.ExecuteDeleteAsync();
                await _context.QuestionAnswerPairs.ExecuteDeleteAsync();
                await _context.Answers.ExecuteDeleteAsync();
                await _context.Questions.ExecuteDeleteAsync();
                await _context.CompetenceSets.ExecuteDeleteAsync();
                await _context.Drafts.ExecuteDeleteAsync();

                await _context.SaveChangesAsync();

                return ServiceResult.Success();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult.Failure("Internal error.");
            }
        }

        public async Task<ServiceResult<Competence>> CreateRandomCompetenceAsync((int, int) answerCountRange, (int, int) answerImpactRange)
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
                return ServiceResult<Competence>.Success(competence);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<Competence>.Failure("Failed to save competence.");
            }
        }

        public async Task<ServiceResult<CompetenceSet>> CreateRandomCompetenceSetAsync(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange)
        {
            try
            {
                var competences = new List<Competence>();

                for(int i=0; i<competenceCount; i++)
                {
                    var result = await CreateRandomCompetenceAsync(answerCountRange, answerImpactRange);
                    if (!result.IsSuccess || result.Data == null)
                    {
                        await Console.Out.WriteLineAsync("Competence failed to create: " + result.Message);
                    }
                    else
                    {
                        competences.Add(result.Data);
                    }
                }

                var competenceSet = new CompetenceSet(competences);
                _context.CompetenceSets.Add(competenceSet);

                await _context.SaveChangesAsync();
                return ServiceResult<CompetenceSet>.Success(competenceSet);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<CompetenceSet>.Failure("Competence set failed to save.");
            }
        }

        public async Task<ServiceResult> DeleteUserDraftsAsync(User user)
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
                return ServiceResult.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return ServiceResult.Failure("Internal error.");
            }
        }

        public async Task<ServiceResult<SubmittedRecord>> FinalizeDraftAsync(User user, string competenceSetId)
        {
            var draft = await _context.Drafts
                .Include(d => d.QuestionAnswerPairs)
                .ThenInclude(qap => qap.Question)
                .Include(d => d.QuestionAnswerPairs)
                .ThenInclude(qap => qap.Answer)
                .FirstOrDefaultAsync(d => d.Author.Id == user.Id);

            if (draft == null)
            {
                return ServiceResult<SubmittedRecord>.Failure("Draft not found.");
            }

            if (draft?.QuestionAnswerPairs == null || !draft.QuestionAnswerPairs.Any())
            {
                return ServiceResult<SubmittedRecord>.Failure("Draft is empty.");
            }

            var competenceValues = new List<CompetenceValue>();
            foreach (var competence in draft.CompetenceSet.Competences)
            {
                if (competence == null || competence.Question == null) { continue; }

                var pair = draft.QuestionAnswerPairs.FirstOrDefault(qap => qap.Question.Id == competence.Question.Id);
                int? numericResult = pair != null ? pair.Answer.InpactOnCompetence : null;

                var newCompValue = new CompetenceValue(competence, numericResult);
                competenceValues.Add(newCompValue);
            }

            var newSubmittedRecord = new SubmittedRecord(user, competenceSetId, competenceValues);
            await _context.SubmittedRecords.AddAsync(newSubmittedRecord);
            await _context.SaveChangesAsync();

            return ServiceResult<SubmittedRecord>.Success(newSubmittedRecord);
        }
        public bool HasQuestionAnswerPairs(Draft draft)
        {
            return draft?.QuestionAnswerPairs != null && draft.QuestionAnswerPairs.Any();
        }

        public async Task<ServiceResult<List<SubmittedRecord>>> GetAllSubmittedRecordsAsync()
        {
            try
            {
                var submittedRecords = await _context.SubmittedRecords
                    .Include(sr => sr.Author)
                    .Include(sr => sr.CompetenceValues)
                    .ThenInclude(cv => cv.Competence)
                    .ToListAsync();

                return ServiceResult<List<SubmittedRecord>>.Success(submittedRecords);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<List<SubmittedRecord>>.Failure("Internal error.");
            }
        }

        public async Task<ServiceResult<int>> GetUnfinishedUserCountAsync()
        {
            try
            {
                var userCount = await _context.Drafts
                    .Select(d => d.Author.Id)
                    .Distinct()
                    .CountAsync();

                return ServiceResult<int>.Success(userCount);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<int>.Failure("Internal error.");
            }
        }

        public async Task<ServiceResult<CompetenceSet>> CreateCompetenceSetFromJsonAsync(CompetenceSetJson jsonData)
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
                    return ServiceResult<CompetenceSet>.Failure("Invalid or empty JSON data.");
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

                return ServiceResult<CompetenceSet>.Success(competenceSet);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                return ServiceResult<CompetenceSet>.Failure("Failed to create competence set from JSON.");
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
