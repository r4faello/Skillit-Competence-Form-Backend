using CompetenceForm.Common;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;

namespace CompetenceForm.Services.UserService
{
    public interface IUserService
    {
        public Task<(ServiceResult, User?)> GetUserByIdAsync(string userId, UserQuery query);
        public Task<(ServiceResult, User?)> CreateUserAsync(string username, string password);
        public Task<(ServiceResult, string)> GenerateJwtAsync(string username, string password);
        bool IsPasswordCorrect(string password, string storedHashedPassword, string storedSalt);
    }
}
