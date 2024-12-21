namespace CompetenceForm.Models
{
    public class Answer
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int InpactOnCompetence { get; set; } = 0;


        public Answer(string title, int impactOnCompetence, string description)
        {
            Title = title;
            InpactOnCompetence = InpactOnCompetence;
            Description = description;
        }
        public Answer(){}
    }   
}
