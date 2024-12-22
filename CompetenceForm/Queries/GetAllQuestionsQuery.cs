using CompetenceForm.Common;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using MediatR;

namespace CompetenceForm.Queries
{
    public class GetAllQuestionsQuery : IRequest<ServiceResult<CompetenceSetDto>>
    {
        public User User { get; }

        public GetAllQuestionsQuery(User user)
        {
            User = user;
        }
    }
}
