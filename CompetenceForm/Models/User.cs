namespace CompetenceForm.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public List<Draft> Drafts { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
            Username = "";
            HashedPassword = "";
            Salt = "";
            IsAdmin = false;
            Drafts = new List<Draft>();
        }

        public User(string id, string username, string password, string salt)
        {
            Id = id;
            Username = username;
            HashedPassword = password;
            Salt = salt;
            IsAdmin = false;
            Drafts = new List<Draft>();
        }
    }
}
