namespace CompetenceForm.Models
{
    public class Draft
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public CompetenceSet CompetenceSet { get; set; }
        public List<QuestionAnswer> Answers { get; set; }
        public DateTime InitiatedAt { get; set; }

        public Draft(User author, CompetenceSet competenceSet)
        {
            Id = Guid.NewGuid().ToString();
            Author = author;
            CompetenceSet = competenceSet;
            Answers = new List<QuestionAnswer>();
            InitiatedAt = DateTime.Now;
        }

        public Draft()
        {
            Id = Guid.NewGuid().ToString();
            Answers = new List<QuestionAnswer>();
            InitiatedAt = DateTime.Now;
        }
    }
}
