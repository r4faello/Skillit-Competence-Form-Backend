namespace CompetenceForm.Models
{
    public class User
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public List<Draft> Drafts { get; set; } = new List<Draft>();


        public User(string username, string password, string salt)
        {
            Username = username;
            HashedPassword = password;
            Salt = salt;
        }
        public User(){}
    }
}
