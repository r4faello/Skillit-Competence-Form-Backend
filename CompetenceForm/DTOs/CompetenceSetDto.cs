namespace CompetenceForm.DTOs
{
    public class CompetenceSetDto
    {
        public string CompetenceSetId { get; set; }
        public List<CompetenceDto> Competences { get; set; }
    }

    public class CompetenceDto
    {
        public string CompetenceId { get; set; }
        public string? Question { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; }
        public string DraftedAnswerId { get; set; }
    }

    public class AnswerOptionDto
    {
        public string AnswerId { get; set; }
        public string? Answer { get; set; }
        public string Description { get; set; }
    }
}
