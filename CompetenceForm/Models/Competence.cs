namespace CompetenceForm.Models
{
    public class Competence
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public Question? Question { get; set; }


        public Competence(Question question, string title = "")
        {
            Title = title;
            Question = question;
        }
        public Competence(){}
    }
}
