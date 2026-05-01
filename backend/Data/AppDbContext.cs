using Microsoft.EntityFrameworkCore;
using MediaDraftLeague.Backend.Domain.Entities;

namespace MediaDraftLeague.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueParticipant> LeagueParticipants { get; set; }
        public DbSet<Draft> Drafts { get; set; }
        public DbSet<DraftPick> DraftPicks { get; set; }
    }
} 