namespace MediaDraftLeague.Backend.Domain.Entities
{
    public class League
    {
        public int Id {get; set; }
        public string Name { get; set; } = string.Empty;
        public int SeasonYear { get; set; }
        public bool IsDraftComplete { get; set; }
    }
} 