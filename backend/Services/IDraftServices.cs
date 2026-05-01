using MediaDraftLeague.Backend.Domain.Entities;

namespace MediaDraftLeague.Backend.Services
{
    public interface IDraftServices
    {
        Task<Draft> StartDraftAsync(int LeagueId, int pickPerParticipant, CancellationToken cancellationToken = default);

        Task<Draft> MakePickAsync(int LeagueId, int LeagueParticipantId, int MediaItemId, CancellationToken cancellationToken = default);
    }
}