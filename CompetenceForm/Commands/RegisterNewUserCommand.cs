using CompetenceForm.Common;
using MediatR;

namespace CompetenceForm.Commands
{
    public class RegisterNewUserCommand : IRequest<ServiceResult<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterNewUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
