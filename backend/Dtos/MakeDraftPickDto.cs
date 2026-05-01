using System.ComponentModel.DataAnnotations;

namespace MediaDraftLeague.Backend.Dtos
{
    public class MakeDraftPickDto
    {
        [Required]
        public int LeagueParticipantId { get; set; }

        [Required]
        public int MediaItemId { get; set; }
    }
}