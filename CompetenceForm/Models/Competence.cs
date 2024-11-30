namespace CompetenceForm.Models
{
    public class Competence
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Question Question { get; set; }
    }
}
