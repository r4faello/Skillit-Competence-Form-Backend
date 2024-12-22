using CompetenceForm.Common;
using CompetenceForm.Queries;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class GetUnfinishedUserCountQueryHandler : IRequestHandler<GetUnfinishedUserCountQuery, ServiceResult<int>>
    {
        private readonly ICompetenceService _competenceService;

        public GetUnfinishedUserCountQueryHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult<int>> Handle(GetUnfinishedUserCountQuery request, CancellationToken cancellationToken)
        {
            return await _competenceService.GetUnfinishedUserCountAsync();
        }
    }

}
