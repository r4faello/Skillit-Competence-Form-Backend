using CompetenceForm.Common;
using CompetenceForm.DTOs;
using MediatR;

namespace CompetenceForm.Queries
{
    public class GetSurveyResultsQuery : IRequest<ServiceResult<List<SubmittedRecordDto>>>{}

}
