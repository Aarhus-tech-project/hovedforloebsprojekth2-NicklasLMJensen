namespace MediaDraftLeague.Backend.Domain.Entities
{
    public class Draft
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public int Rounds { get; set; }
        public int PicksPerParticipant { get; set; }
        public int CurrentPickNumber { get; set; }

    }
}