namespace CompetenceForm.Models
{
    public class Competence
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Question? Question { get; set; }


        public Competence(Question question, string title = "")
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Question = question;
        }

        public Competence()
        {
            Id = Guid.NewGuid().ToString();
            Title = "";
        }
    }
}
