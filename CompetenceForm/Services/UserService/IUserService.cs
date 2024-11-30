using CompetenceForm.Common;
using CompetenceForm.Models;

namespace CompetenceForm.Services.UserService
{
    public interface IUserService
    {
        public Task<(Result, User?)> CreateUserAsync(string username, string password);
        public Task<(Result, string)> GenerateJwtAsync(string username, string password);
        bool IsPasswordCorrect(string password, string storedHashedPassword, string storedSalt);
    }
}
