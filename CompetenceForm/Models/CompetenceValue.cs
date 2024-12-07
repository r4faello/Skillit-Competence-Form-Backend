using Microsoft.Identity.Client.Extensibility;

namespace CompetenceForm.Models
{
    public class CompetenceValue
    {
        public string Id { get; set; }
        public Competence? Competence { get; set; }
        public int? Value { get; set; }


        public CompetenceValue()
        {
            Id = Guid.NewGuid().ToString();
            Competence = null;
            Value = null;
        }

        public CompetenceValue(Competence competence, int? value)
        {
            Id = Guid.NewGuid().ToString();
            Competence = competence;
            Value = value;
        }
    }
}
