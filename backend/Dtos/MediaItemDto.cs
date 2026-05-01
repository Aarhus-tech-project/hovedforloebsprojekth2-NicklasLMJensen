using System.ComponentModel.DataAnnotations;
using MediaDraftLeague.Backend.Domain.Entities;


namespace MediaDraftLeague.Backend.Dtos
{
    public class MediaItemDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;
        public MediaType Type { get; set; }
        public double? Score { get; set; }
    }
}