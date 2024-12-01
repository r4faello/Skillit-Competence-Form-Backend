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
    }
}
