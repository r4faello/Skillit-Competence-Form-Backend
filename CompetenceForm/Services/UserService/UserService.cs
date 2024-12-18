using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.AuthService;
using CompetenceForm.Services.PasswordService;

namespace CompetenceForm.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IAuthService authService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<(Result, User?)> GetUserByIdAsync(string userId, UserQuery query)
        {

            var user = await _userRepository.GetUserByIdAsync(userId, query);
            if(user == null)
            {
                return (Result.Failure("User not found"), null);
            }

            return (Result.Success(), user);
        }

        public async Task<(Result, User?)> CreateUserAsync(string username, string password)
        {
            // Validate password strength
            if (!_passwordService.IsPasswordStrongEnough(password))
            {
                return (Result.Failure("Password is not strong enough. It should have at least 12 chars, upper, lower letters, numbers and special characters."), null);
            }

            // Check if username is free to use
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                return (Result.Failure("User with specified username already exists"), null);
            }

            // Hashing password
            var (hashedPassword, salt) = _passwordService.HashPassword(password);


            var newUser = new User(Guid.NewGuid().ToString(), username, hashedPassword, salt);
            await _userRepository.AddAsync(newUser);
            return (Result.Success(), newUser);
        }

        public async Task<(Result, string)> GenerateJwtAsync(string username, string password)
        {
            // Getting user
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return (Result.Failure("User with specified username does not exist"), string.Empty);
            }

            // Checking if password is correct
            if (!IsPasswordCorrect(password, user.HashedPassword, user.Salt))
            {
                return (Result.Failure("Password is not correct"), string.Empty);
            }
            return (Result.Success(), _authService.GenerateJwtToken(user));
        }


        public bool IsPasswordCorrect(string password, string storedHashedPassword, string storedSalt)
        {
            var hashedInputPassword = _passwordService.HashPassword(password, storedSalt);
            return hashedInputPassword == storedHashedPassword;
        }
    }
}
