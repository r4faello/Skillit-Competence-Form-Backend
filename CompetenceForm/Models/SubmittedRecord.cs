using Microsoft.AspNetCore.Authentication;

namespace CompetenceForm.Models
{
    public class SubmittedRecord
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }
        public User? Author { get; set; }

        public string CompetenceSetId { get; set; }

        public List<CompetenceValue> CompetenceValues { get; set; }


        public DateTime SubmittedAt { get; set; }



        public SubmittedRecord()
        {
            Id = Guid.NewGuid().ToString();
            Author = null;
            CompetenceSetId = "";
            CompetenceValues = new List<CompetenceValue>();
            SubmittedAt = DateTime.Now;
        }

        public SubmittedRecord(User? author, string competenceSetId, List<CompetenceValue> competenceValues)
        {
            Id = Guid.NewGuid().ToString();
            Author = author;
            CompetenceSetId = competenceSetId;
            CompetenceValues = competenceValues;
        }
    }
}
