using CompetenceForm.Common;
using MediatR;

namespace CompetenceForm.Commands
{
    public class LoginUserCommand : IRequest<ServiceResult<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
