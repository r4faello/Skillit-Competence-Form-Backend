using CompetenceForm.Common;
using CompetenceForm.Models;
using MediatR;

namespace CompetenceForm.Commands
{
    public class DeleteDraftCommand : IRequest<ServiceResult>
    {
        public User User { get; set; }

        public DeleteDraftCommand(User user)
        {
            User = user;
        }
    }
}
