using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class DeleteDraftCommandHandler : IRequestHandler<DeleteDraftCommand, ServiceResult>
    {
        private readonly ICompetenceService _competenceService;

        public DeleteDraftCommandHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult> Handle(DeleteDraftCommand request, CancellationToken cancellationToken)
        {
            return await _competenceService.DeleteUserDraftsAsync(request.User);
        }
    }
}
