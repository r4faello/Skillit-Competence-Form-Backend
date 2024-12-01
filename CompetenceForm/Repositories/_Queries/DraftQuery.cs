namespace CompetenceForm.Repositories._Queries
{
    public class DraftQuery
    {
        public bool IncludeAuthor { get; set; }
        public bool IncludeCompetenceSet { get; set; }
        public bool IncludeQuestionAnswerPairs { get; set; }
        public bool IncludeQuestionAnswerPairQuestion { get; set; }
        public bool IncludeQuestionAnswerPairAnswer { get; set; }
    }
}
