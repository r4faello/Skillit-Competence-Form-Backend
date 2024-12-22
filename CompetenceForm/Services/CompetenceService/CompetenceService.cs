using AutoMapper;
using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.CompetenceService;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Services.CompetenceService;

public class CompetenceService : ICompetenceService
{
    private readonly ICompetenceRepository _competenceRepository;

    public CompetenceService(ICompetenceRepository competenceRepository)
    {
        _competenceRepository = competenceRepository;
    }

    public async Task<ServiceResult> SaveAnsweredQuestionAsync(User user, string competenceSetId, string competenceId, string answerId)
    {
        // Retrieve and validate the competence set
        var competenceSetQuery = new CompetenceSetQuery
        {
            IncludeCompetences = true,
            IncludeCompetenceQuestion = true,
            IncludeCompetenceQuestionAnswerOptions = true
        };

        var competenceSetResult = await _competenceRepository.GetCompetenceSetByIdAsync(competenceSetId, competenceSetQuery);
        if (!competenceSetResult.IsSuccess || competenceSetResult.Data == null)
        {
            return ServiceResult.Failure("Invalid competence set ID.");
        }

        var competence = competenceSetResult.Data.Competences.FirstOrDefault(c => c.Id == competenceId);
        if (competence == null || competence.Question == null)
        {
            return ServiceResult.Failure("Invalid competence or missing question.");
        }

        var answer = competence.Question.AnswerOptions.FirstOrDefault(a => a.Id == answerId);
        if (answer == null)
        {
            return ServiceResult.Failure("Answer option not found.");
        }

        // Retrieve or create the draft
        var draft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == competenceSetId);
        if (draft == null)
        {
            var draftCreationResult = await _competenceRepository.CreateNewDraftAsync(user, competenceSetResult.Data);
            if (!draftCreationResult.IsSuccess || draftCreationResult.Data == null)
            {
                return draftCreationResult;
            }
            draft = draftCreationResult.Data;
        }

        // Load the full draft details
        var draftQuery = new DraftQuery
        {
            IncludeQuestionAnswerPairs = true,
            IncludeQuestionAnswerPairQuestion = true,
            IncludeQuestionAnswerPairAnswer = true,
            IncludeAuthor = true
        };

        var draftResult = await _competenceRepository.GetDraftByIdAsync(draft.Id, draftQuery);
        if (!draftResult.IsSuccess || draftResult.Data == null)
        {
            return draftResult;
        }

        await _competenceRepository.RegisterAnsweredQuestionAsync(draftResult.Data, competence.Question, answer);

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<CompetenceSetDto>> GetCompetenceSetAsync(User user)
    {
        var competenceSetResult = await _competenceRepository.GetCurrentCompetenceSetAsync();
        if (!competenceSetResult.IsSuccess || competenceSetResult.Data == null)
        {
            return ServiceResult<CompetenceSetDto>.Failure("Competence set not found.");
        }

        var currentDraft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == competenceSetResult.Data.Id);
        if (currentDraft != null)
        {
            var draftQuery = new DraftQuery
            {
                IncludeQuestionAnswerPairs = true,
                IncludeQuestionAnswerPairQuestion = true,
                IncludeQuestionAnswerPairAnswer = true
            };

            var draftResult = await _competenceRepository.GetDraftByIdAsync(currentDraft.Id, draftQuery);
            if (draftResult.IsSuccess)
            {
                currentDraft = draftResult.Data;
            }
        }

        var competenceSetDto = new CompetenceSetDto
        {
            CompetenceSetId = competenceSetResult.Data.Id,
            Competences = competenceSetResult.Data.Competences
                .Where(c => c.Question != null)
                .Select(c => new CompetenceDto
                {
                    CompetenceId = c.Id,
                    Question = c.Question.Title,
                    AnswerOptions = c.Question.AnswerOptions.Select(a => new AnswerOptionDto
                    {
                        AnswerId = a.Id,
                        Answer = a.Title,
                        Description = a.Description
                    }).ToList(),
                    DraftedAnswerId = currentDraft?.QuestionAnswerPairs
                        .FirstOrDefault(qap => qap.Question.Id == c.Question.Id)?.Answer.Id ?? string.Empty
                }).ToList()
        };

        return ServiceResult<CompetenceSetDto>.Success(competenceSetDto);
    }

    public async Task<ServiceResult> SeedCompetencesAsync(int competenceCount, (int, int) answerCountRange, (int, int) answerImpactRange)
    {
        var wipeResult = await _competenceRepository.WipeCompetenceSetsAsync();
        if (!wipeResult.IsSuccess)
        {
            return ServiceResult.Failure("Failed to delete existing competence sets.");
        }

        var createResult = await _competenceRepository.CreateRandomCompetenceSetAsync(competenceCount, answerCountRange, answerImpactRange);
        return createResult.IsSuccess ? ServiceResult.Success() : ServiceResult.Failure("Failed to seed competence sets.");
    }

    public async Task<ServiceResult<SubmittedRecordDto>> FinalizeDraftAsync(User user)
    {
        if (user == null)
        {
            return ServiceResult<SubmittedRecordDto>.Failure("Invalid user.");
        }

        var competenceSetResult = await _competenceRepository.GetCurrentCompetenceSetAsync();
        if (!competenceSetResult.IsSuccess || competenceSetResult.Data == null)
        {
            return ServiceResult<SubmittedRecordDto>.Failure("Competence set not found.");
        }

        var finalizeResult = await _competenceRepository.FinalizeDraftAsync(user, competenceSetResult.Data.Id);
        if (!finalizeResult.IsSuccess || finalizeResult.Data == null)
        {
            return ServiceResult<SubmittedRecordDto>.Failure(finalizeResult.Message);
        }

        var deleteResult = await _competenceRepository.DeleteUserDraftsAsync(user);
        if (!deleteResult.IsSuccess)
        {
            return ServiceResult<SubmittedRecordDto>.Failure(deleteResult.Message);
        }

        var submittedRecordDto = new SubmittedRecordDto
        {
            RecordId = finalizeResult.Data.Id,
            AuthorId = finalizeResult.Data.AuthorId,
            AuthorUsername = finalizeResult.Data.Author?.Username ?? "Unknown User",
            Competences = finalizeResult.Data.CompetenceValues.Select(cv => new CompetenceValueDto
            {
                CompetenceId = cv.Competence?.Id ?? string.Empty,
                CompetenceTitle = cv.Competence?.Title ?? "Unknown Competence",
                Value = cv.Value
            }).ToList(),
            SubmittedAt = finalizeResult.Data.SubmittedAt
        };

        return ServiceResult<SubmittedRecordDto>.Success(submittedRecordDto);
    }

    public async Task<ServiceResult<List<SubmittedRecordDto>>> GetSubmittedRecordsAsync()
    {
        var submittedRecordsResult = await _competenceRepository.GetAllSubmittedRecordsAsync();
        if (!submittedRecordsResult.IsSuccess || submittedRecordsResult.Data == null || !submittedRecordsResult.Data.Any())
        {
            return ServiceResult<List<SubmittedRecordDto>>.Failure("No submitted records found.");
        }

        var submittedRecords = submittedRecordsResult.Data.Select(record => new SubmittedRecordDto
        {
            RecordId = record.Id,
            AuthorId = record.AuthorId,
            AuthorUsername = record.Author?.Username ?? "Unknown User",
            Competences = record.CompetenceValues.Select(cv => new CompetenceValueDto
            {
                CompetenceId = cv.Competence?.Id ?? string.Empty,
                CompetenceTitle = cv.Competence?.Title ?? "Unknown Competence",
                Value = cv.Value
            }).ToList(),
            SubmittedAt = record.SubmittedAt
        }).ToList();

        return ServiceResult<List<SubmittedRecordDto>>.Success(submittedRecords);
    }

    public async Task<ServiceResult<int>> GetUnfinishedUserCountAsync()
    {
        var result = await _competenceRepository.GetUnfinishedUserCountAsync();
        return result.IsSuccess
            ? ServiceResult<int>.Success(result.Data)
            : ServiceResult<int>.Failure(result.Message);
    }

    public async Task<ServiceResult> SeedCompetenceSetFromJsonAsync(CompetenceSetJson jsonData)
    {
        var wipeResult = await _competenceRepository.WipeCompetenceSetsAsync();
        if (!wipeResult.IsSuccess)
        {
            return wipeResult;
        }

        var createResult = await _competenceRepository.CreateCompetenceSetFromJsonAsync(jsonData);
        return createResult.IsSuccess
            ? ServiceResult.Success()
            : ServiceResult.Failure("Failed to seed competence set.");
    }

    public Task<ServiceResult> DeleteUserDraftsAsync(User user)
    {
        return _competenceRepository.DeleteUserDraftsAsync(user);
    }

    public async Task<string> GetCurrentCompetenceSetIdAsync()
    {
        var result = await _competenceRepository.GetCurrentCompetenceSetAsync();
        return result.IsSuccess && result.Data != null ? result.Data.Id : string.Empty;
    }
}
