namespace CompetenceForm.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int InpactOnCompetence { get; set; }


        public Answer()
        {
            Id = Guid.NewGuid().ToString();
            Title = "";
            InpactOnCompetence = 0;
        }

        public Answer(string title, int impactOnCompetence)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            InpactOnCompetence = InpactOnCompetence;
        }
    }   
}
