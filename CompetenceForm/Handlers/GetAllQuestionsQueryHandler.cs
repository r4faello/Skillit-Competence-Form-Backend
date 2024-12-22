using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Queries;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, ServiceResult<CompetenceSetDto>>
    {
        private readonly ICompetenceService _competenceService;

        public GetAllQuestionsQueryHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult<CompetenceSetDto>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var user = request.User;

            var result = await _competenceService.GetCompetenceSetAsync(user);
            if (!result.IsSuccess)
            {
                return ServiceResult<CompetenceSetDto>.Failure(result.Message);
            }

            return result;
        }
    }
}
