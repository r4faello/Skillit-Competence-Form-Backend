using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using Microsoft.EntityFrameworkCore;

namespace CompetenceForm.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(string id, UserQuery query)
        {
            var userQuery = _context.Users.AsQueryable();

            if (query.IncludeDrafts)
            {
                userQuery = userQuery.Include(u => u.Drafts);
            }


            return await userQuery.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}
