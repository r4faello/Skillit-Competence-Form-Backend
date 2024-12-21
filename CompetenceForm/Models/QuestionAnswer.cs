namespace CompetenceForm.Models
{
    public class QuestionAnswer
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public Question Question { get; set; }
        public Answer Answer { get; set; }

        public QuestionAnswer(Question question, Answer answer)
        {
            Question = question;
            Answer = answer;
        }

        public QuestionAnswer(){}
    }
}
