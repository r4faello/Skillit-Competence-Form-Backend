namespace CompetenceForm.Models
{
    public class CompetenceSet
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public List<Competence> Competences { get; set; } = new List<Competence>();


        public CompetenceSet(List<Competence> competences)
        {
            Competences = competences;
        }
        public CompetenceSet(){}
    }
}
