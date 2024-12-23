using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class SaveAnsweredQuestionCommandHandler : IRequestHandler<SaveAnsweredQuestionCommand, ServiceResult>
    {
        private readonly ICompetenceService _competenceService;

        public SaveAnsweredQuestionCommandHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult> Handle(SaveAnsweredQuestionCommand request, CancellationToken cancellationToken)
        {
            return await _competenceService.SaveAnsweredQuestionAsync(
                request.User,
                request.CompetenceSetId,
                request.CompetenceId,
                request.AnswerId);
        }
    }

}
