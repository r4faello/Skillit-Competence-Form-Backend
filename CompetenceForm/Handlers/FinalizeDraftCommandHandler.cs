using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class FinalizeDraftCommandHandler : IRequestHandler<FinalizeDraftCommand, ServiceResult<SubmittedRecordDto>>
    {
        private readonly ICompetenceService _competenceService;

        public FinalizeDraftCommandHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult<SubmittedRecordDto>> Handle(FinalizeDraftCommand request, CancellationToken cancellationToken)
        {
            return await _competenceService.FinalizeDraftAsync(request.User);
        }
    }
}
