namespace CompetenceForm.Models
{
    public class CompetenceSet
    {
        public string Id { get; set; }
        public List<Competence> Competences { get; set; }


        public CompetenceSet()
        {
            Id = Guid.NewGuid().ToString();
            Competences = new List<Competence>();
        }

        public CompetenceSet(List<Competence> competences)
        {
            Id = Guid.NewGuid().ToString();
            Competences = competences;
        }
    }
}
