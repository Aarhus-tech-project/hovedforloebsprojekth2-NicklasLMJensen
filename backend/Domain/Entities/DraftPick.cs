namespace MediaDraftLeague.Backend.Domain.Entities
{
    public class DraftPick
    {
        public int Id { get; set; }
        public int DraftId { get; set; }
        public int LeagueParticipantId { get; set; }
        public int MediaItemId { get; set; }
        public int PickNumber { get; set; }
    }
}