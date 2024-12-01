using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;

namespace CompetenceForm.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByIdAsync(string id, UserQuery query);
        public Task<User?> GetByUsernameAsync(string username);
        public Task AddAsync(User user);
    }
}
