using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.UserService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class RegisterNewUserHandler : IRequestHandler<RegisterNewUserCommand, ServiceResult<string>>
    {
        private readonly IUserService _userService;

        public RegisterNewUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ServiceResult<string>> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            var (createResult, _) = await _userService.CreateUserAsync(request.Username, request.Password);
            if (!createResult.IsSuccess)
            {
                return ServiceResult<string>.Failure(createResult.Message);
            }

            var (authResult, jwtToken) = await _userService.GenerateJwtAsync(request.Username, request.Password);
            if (!authResult.IsSuccess)
            {
                return ServiceResult<string>.Failure(authResult.Message);
            }

            // Return a success result with the JWT token
            return ServiceResult<string>.Success(jwtToken);
        }

    }


}
