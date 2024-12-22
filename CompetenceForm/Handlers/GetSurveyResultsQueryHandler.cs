using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Queries;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class GetSurveyResultsQueryHandler : IRequestHandler<GetSurveyResultsQuery, ServiceResult<List<SubmittedRecordDto>>>
    {
        private readonly ICompetenceService _competenceService;

        public GetSurveyResultsQueryHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult<List<SubmittedRecordDto>>> Handle(GetSurveyResultsQuery request, CancellationToken cancellationToken)
        {
            return await _competenceService.GetSubmittedRecordsAsync();
        }
    }
}
