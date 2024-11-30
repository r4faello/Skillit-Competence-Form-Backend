namespace CompetenceForm.Services.PasswordService
{
    public interface IPasswordService
    {
        (string hash, string salt) HashPassword(string password);
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
        bool IsPasswordStrongEnough(string password);
    }
}
