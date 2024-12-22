using CompetenceForm.Commands;
using CompetenceForm.Common;
using CompetenceForm.Services.UserService;
using MediatR;

namespace CompetenceForm.Handlers
{
    public class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, ServiceResult<string>>
    {
        private readonly IUserService _userService;

        public RegisterNewUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ServiceResult<string>> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            var createResult = await _userService.CreateUserAsync(request.Username, request.Password);
            if (!createResult.IsSuccess)
            {
                return ServiceResult<string>.Failure(createResult.Message);
            }

            var authResult = await _userService.GenerateJwtAsync(request.Username, request.Password);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return ServiceResult<string>.Failure(authResult.Message);
            }

            return ServiceResult<string>.Success(authResult.Data);
        }

    }


}
