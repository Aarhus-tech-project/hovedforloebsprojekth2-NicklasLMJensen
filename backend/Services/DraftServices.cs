using Microsoft.EntityFrameworkCore;
using MediaDraftLeague.Backend.Data;
using MediaDraftLeague.Backend.Domain.Entities;
using MediaDraftLeague.Backend.Services;

namespace MediaDraftleague.Backend.Services
{
    public class DraftServices : IDraftServices
    {
        private readonly AppDbContext _db;

        public DraftServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Draft> StartDraftAsync(int LeagueId, int PicksPerParticipant, CancellationToken cancellationToken = default)
        {
            var league = await _db.Leagues
                .FirstOrDefaultAsync(l => l.Id == LeagueId, cancellationToken);

            if (league is null)
            {
                throw new InvalidOperationException($"League with id {LeagueId} not found.");
            }

            var participants = await _db.LeagueParticipants
                .Where(p => p.LeagueId == LeagueId)
                .ToListAsync(cancellationToken);

            if (participants.Count == 0)
            {
                throw new InvalidOperationException("Cannot start draft without participants.");
            }

            var existingDraft = await _db.Drafts
                .FirstOrDefaultAsync(d => d.LeagueId == LeagueId, cancellationToken);

            if (existingDraft is not null)
            {
                throw new KeyNotFoundException("This league is already in a draft.");
            }

            var draft = new Draft
            {
                LeagueId = LeagueId,
                Rounds = PicksPerParticipant,
                PicksPerParticipant = PicksPerParticipant,
                CurrentPickNumber = 1,
            };

            _db.Drafts.Add(draft);
            await _db.SaveChangesAsync(cancellationToken);

            return draft;
        }

    }
}