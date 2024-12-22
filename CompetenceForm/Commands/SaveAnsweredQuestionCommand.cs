using CompetenceForm.Common;
using CompetenceForm.Models;
using MediatR;

namespace CompetenceForm.Commands
{
    public class SaveAnsweredQuestionCommand : IRequest<ServiceResult>
    {
        public User User { get; set; }
        public string CompetenceSetId { get; set; }
        public string CompetenceId { get; set; }
        public string AnswerId { get; set; }

        public SaveAnsweredQuestionCommand(User user, string competenceSetId, string competenceId, string answerId)
        {
            User = user;
            CompetenceSetId = competenceSetId;
            CompetenceId = competenceId;
            AnswerId = answerId;
        }
    }

}
