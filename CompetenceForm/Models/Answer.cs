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
    }   
}
