using CompetenceForm.Common;
using CompetenceForm.DTOs;

namespace CompetenceForm.Services.CompetenceService
{
    public interface ICompetenceService
    {
        public Task<(Result, CompetenceSetDto?)> SpitCompetenceSet();
    }
}
