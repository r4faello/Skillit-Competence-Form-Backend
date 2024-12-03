namespace CompetenceForm.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int InpactOnCompetence { get; set; }


        public Answer()
        {
            Id = Guid.NewGuid().ToString();
            Title = "";
            InpactOnCompetence = 0;
            Description = "";
        }

        public Answer(string title, int impactOnCompetence, string description)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            InpactOnCompetence = InpactOnCompetence;
            Description = description;
        }
    }   
}
