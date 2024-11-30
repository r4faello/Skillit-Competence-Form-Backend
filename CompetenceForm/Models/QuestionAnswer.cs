namespace CompetenceForm.Models
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }

        public QuestionAnswer(Question question, Answer answer)
        {
            Id = Guid.NewGuid().ToString();
            Question = question;
            Answer = answer;
        }

        public QuestionAnswer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
