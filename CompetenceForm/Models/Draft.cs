namespace CompetenceForm.Models
{
    public class Draft
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public CompetenceSet Competences { get; set; }
        public List<Answer?> Answers { get; set; }
        public DateTime Initiated { get; set; }
    }
}
