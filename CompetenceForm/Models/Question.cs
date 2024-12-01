namespace CompetenceForm.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Answer> AnswerOptions { get; set; }

        public Question()
        {
            Id = Guid.NewGuid().ToString();
            Title = "";
            AnswerOptions = new List<Answer>();
        }
    }
}
