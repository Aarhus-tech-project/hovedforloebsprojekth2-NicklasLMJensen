using System.ComponentModel.DataAnnotations;

namespace MediaDraftLeague.Backend.Dtos
{
    public class StartDraftDto
    {
        [Range(1,50, ErrorMessage = "Picks per participant must be between 1 and 50.")]
        public int PicksPerParticipant { get; set;}
    }
}