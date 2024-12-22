using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.CompetenceService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class SeedCompetencesCommandHandler : IRequestHandler<SeedCompetencesCommand, ServiceResult>
    {
        private readonly ICompetenceService _competenceService;

        public SeedCompetencesCommandHandler(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
        }

        public async Task<ServiceResult> Handle(SeedCompetencesCommand request, CancellationToken cancellationToken)
        {
            return await _competenceService.SeedCompetenceSetFromJsonAsync(request.JsonData);
        }
    }

}
