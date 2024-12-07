namespace CompetenceForm.Models
{
    public class Draft
    {
        public string Id { get; set; }
        public User Author { get; set; }

        public string CompetenceSetId { get; set; }
        public CompetenceSet CompetenceSet { get; set; }
        public List<QuestionAnswer> QuestionAnswerPairs { get; set; }
        public DateTime InitiatedAt { get; set; }

        public Draft(User author, CompetenceSet competenceSet)
        {
            Id = Guid.NewGuid().ToString();
            Author = author;
            CompetenceSet = competenceSet;
            QuestionAnswerPairs = new List<QuestionAnswer>();
            InitiatedAt = DateTime.Now;
        }

        public Draft()
        {
            Id = Guid.NewGuid().ToString();
            QuestionAnswerPairs = new List<QuestionAnswer>();
            InitiatedAt = DateTime.Now;
        }
    }
}
