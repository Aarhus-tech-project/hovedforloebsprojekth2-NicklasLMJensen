namespace MediaDraftLeague.Backend.Domain.Entities
{
    public class LeagueParticipant
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public string TeamName { get; set; } = string.Empty;
    }
}