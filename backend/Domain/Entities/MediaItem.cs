using Microsoft.AspNetCore.Components.Web;

namespace MediaDraftLeague.Backend.Domain.Entities
{
    public enum MediaType
    {
        Movie = 1,
        tvShow = 2,
        Game = 3,
        Anime = 4,
    }

    public class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public MediaType Type { get; set; }
        public double? Score { get; set; }
    }
}