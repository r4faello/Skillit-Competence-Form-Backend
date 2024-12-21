namespace CompetenceForm.Models
{
    public class Question
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public List<Answer> AnswerOptions { get; set; } = new List<Answer>();


        public Question(string title, List<Answer> answerOptions)
        {
            Title = title;
            AnswerOptions = answerOptions;
        }
        public Question() {}
    }
}
