using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.UserService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ServiceResult<string>>
    {
        private readonly IUserService _userService;

        public LoginUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ServiceResult<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.GenerateJwtAsync(request.Username, request.Password);
        }
    }

}
