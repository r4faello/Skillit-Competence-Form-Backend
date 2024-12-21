namespace CompetenceForm.Models
{
    public class Draft
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public User Author { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public CompetenceSet CompetenceSet { get; set; }
        public string CompetenceSetId { get; set; } = string.Empty;
        public List<QuestionAnswer> QuestionAnswerPairs { get; set; } = new List<QuestionAnswer>();
        public DateTime InitiatedAt { get; set; } = DateTime.Now;

        public Draft(User author, CompetenceSet competenceSet)
        {
            Author = author;
            CompetenceSet = competenceSet;
            QuestionAnswerPairs = new List<QuestionAnswer>();
            InitiatedAt = DateTime.Now;
        }

        public Draft(){}
    }
}
