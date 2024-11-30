namespace CompetenceForm.Controllers
{
    public class SaveAnsweredQuestionDto
    {
        public string CompetenceSetId { get; set; }
        public string CompetenceId { get; set; }
        public string AnswerId { get; set; }
    }
}