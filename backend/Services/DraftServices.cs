using Microsoft.EntityFrameworkCore;
using MediaDraftLeague.Backend.Data;
using MediaDraftLeague.Backend.Domain.Entities;
using MediaDraftLeague.Backend.Services;
using MediaDraftLeague.Backend.Dtos;

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

        public async Task<Draft> MakePickAsync(int LeagueId, int LeagueParticipantId, int MediaItemId, CancellationToken cancellationToken = default)
        {
            var draft = await _db.Drafts
                .FirstOrDefaultAsync(d => d.LeagueId == LeagueId, cancellationToken);

            if (draft is null)
            {
                throw new KeyNotFoundException($"No draft exists for league {LeagueId}.");
            }

            var participants = await _db.LeagueParticipants
                .Where(p => p.LeagueId == LeagueId)
                .OrderBy(p => p.Id)
                .ToListAsync(cancellationToken);

            if (participants.Count == 0)
            {
                throw new InvalidOperationException("Cannot make picks without participants.");
            }

            var participant = participants.FirstOrDefault(p => p.Id == LeagueParticipantId);

            if (participant is null)
            {
                throw new InvalidOperationException("Participant does not belong in this leage.");
            }

            var mediaItem = await _db.MediaItems
                .FirstOrDefaultAsync(m => m.Id == MediaItemId, cancellationToken);

            if (mediaItem is null)
            {
                throw new KeyNotFoundException($"Media item {MediaItemId} not found.");
            }

            var totalPicks = participants.Count * draft.PicksPerParticipant;

            if (draft.CurrentPickNumber > totalPicks)
            {
                throw new InvalidOperationException("The draft is already complete.");
            }

            var alreadyPicked = await _db.DraftPicks
                .AnyAsync(p => p.DraftId == draft.Id && p.MediaItemId == MediaItemId, cancellationToken);


            if (alreadyPicked)
            {
                throw new InvalidOperationException("This media item has already been picked.");
            }


            var teams = participants.Count;

            var zeroBasedIndex = draft.CurrentPickNumber - 1;
            var roundIndex = zeroBasedIndex / teams;
            var indexInRound = zeroBasedIndex % teams;

            int participantIndexInOrder;

            if (roundIndex % 2 == 0)
            {
                participantIndexInOrder = indexInRound;           
            }
            else
            {
                participantIndexInOrder = teams - 1 - indexInRound;
            }

            var participantWhoseTurnItIs = participants[participantIndexInOrder];

            if (participantWhoseTurnItIs.Id != LeagueParticipantId)
            {
                throw new InvalidOperationException($"It is not this participant's turn. It is participant {participantWhoseTurnItIs.Id}'s turn.");
            }

            var pick = new DraftPick
            {
                DraftId = draft.Id,
                LeagueParticipantId = LeagueParticipantId,
                MediaItemId = mediaItem.Id,
                PickNumber = draft.CurrentPickNumber,
            };

            _db.DraftPicks.Add(pick);

            draft.CurrentPickNumber++;

            if (draft.CurrentPickNumber > totalPicks)
            {
                var league =await _db.Leagues
                    .FirstOrDefaultAsync(l => l.Id == LeagueId, cancellationToken);

                if (league is not null)
                {
                    league.IsDraftComplete = true;
                }
            }

            await _db.SaveChangesAsync(cancellationToken);

            return draft;

        }

    }
}