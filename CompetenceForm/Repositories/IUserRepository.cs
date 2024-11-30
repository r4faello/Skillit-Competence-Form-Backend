using CompetenceForm.Models;

namespace CompetenceForm.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetByIdAsync(string id);
        public Task<User?> GetByUsernameAsync(string username);
        public Task AddAsync(User user);
    }
}
