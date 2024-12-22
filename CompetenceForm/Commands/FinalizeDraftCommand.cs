using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using MediatR;

namespace CompetenceForm.Commands
{
    public class FinalizeDraftCommand : IRequest<ServiceResult<SubmittedRecordDto>>
    {
        public User User { get; set; }

        public FinalizeDraftCommand(User user)
        {
            User = user;
        }
    }
}
