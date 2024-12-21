using Microsoft.Identity.Client.Extensibility;

namespace CompetenceForm.Models
{
    public class CompetenceValue
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public Competence? Competence { get; set; }
        public int? Value { get; set; }


        public CompetenceValue(Competence competence, int? value)
        {
            Competence = competence;
            Value = value;
        }
        public CompetenceValue(){}
    }
}
