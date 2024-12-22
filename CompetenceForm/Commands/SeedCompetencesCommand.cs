using CompetenceForm.Common;
using MediatR;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Commands
{
    public class SeedCompetencesCommand : IRequest<ServiceResult>
    {
        public CompetenceSetJson JsonData { get; set; }

        public SeedCompetencesCommand(CompetenceSetJson jsonData)
        {
            JsonData = jsonData;
        }
    }

}
