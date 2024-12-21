namespace CompetenceForm.Models
{
    public class SubmittedRecord
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public User? Author { get; set; }
        public string? AuthorId { get; set; }
        public string CompetenceSetId { get; set; } = string.Empty;
        public List<CompetenceValue> CompetenceValues { get; set; } = new List<CompetenceValue>();
        public DateTime SubmittedAt { get; set; } = DateTime.Now;


        public SubmittedRecord(User? author, string competenceSetId, List<CompetenceValue> competenceValues)
        {
            Id = Guid.NewGuid().ToString();
            Author = author;
            CompetenceSetId = competenceSetId;
            CompetenceValues = competenceValues;
        }
        public SubmittedRecord(){}
    }
}
