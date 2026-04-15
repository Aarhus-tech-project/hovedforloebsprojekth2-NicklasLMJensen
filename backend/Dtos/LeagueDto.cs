using System.ComponentModel.DataAnnotations;
using MediaDraftLeague.Backend.Domain.Entities;


namespace MediaDraftLeague.Backend.Dtos
{
    public class LeagueDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Range(1890, 3000, ErrorMessage = "Season year must be between 1890 and 3000.")]
        public int SeasonYear { get; set; }
    }
}