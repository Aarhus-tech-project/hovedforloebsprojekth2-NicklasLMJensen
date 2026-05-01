using System.ComponentModel.DataAnnotations;

namespace MediaDraftLeague.Backend.Dtos;

public class LeagueParticipantsDto
{
    [Required(ErrorMessage = "Team name is required.")]
    public string TeamName { get; set; } = string.Empty;
}