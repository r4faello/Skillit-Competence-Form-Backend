using CompetenceForm.Models;

namespace CompetenceForm.Services.AuthService
{
    public interface IAuthService
    {
        public string GenerateJwtToken(User user);
    }
}
