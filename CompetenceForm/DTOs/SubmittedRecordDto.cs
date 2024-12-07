namespace CompetenceForm.DTOs
{
    public class SubmittedRecordDto
    {
        public string RecordId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public List<CompetenceValueDto> Competences { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class CompetenceValueDto
    {
        public string CompetenceId { get; set; }
        public string CompetenceTitle { get; set; }
        public int? Value { get; set; }
    }

}
