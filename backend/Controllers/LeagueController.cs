using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaDraftLeague.Backend.Data;
using MediaDraftLeague.Backend.Domain.Entities;
using MediaDraftLeague.Backend.Dtos;
using System.Runtime.Versioning;
using MediaDraftLeague.Backend.Services;


namespace MediaDraftLeague.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LeagueController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IDraftServices _draftServices;
        public LeagueController(AppDbContext db, IDraftServices draftServices)
        {
            _db = db;
            _draftServices = draftServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<League>>> GetAll()
        {
            var leagues = await _db.Leagues
                .AsNoTracking()
                .ToListAsync();
            return Ok(leagues);
        }


        [HttpPost]
        public async Task<ActionResult<League>> Create(LeagueDto dto)
        {
            var league = new League
            {
                Name = dto.Name,
                SeasonYear = dto.SeasonYear,
                IsDraftComplete = false
            };

            _db.Leagues.Add(league);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = league.Id }, league);
        }

        [HttpGet("{LeagueId}/participants")]
        public async Task<ActionResult<IEnumerable<LeagueParticipant>>> GetParticipants(int LeagueId)
        {
            var participants = await _db.LeagueParticipants
                .AsNoTracking()
                .Where(p => p.LeagueId == LeagueId)
                .ToListAsync();

            return Ok(participants);
        }

        [HttpPost("{LeagueId:int}/participants")]
        public async Task<ActionResult<LeagueParticipant>> AddParticipant(int LeagueId, LeagueParticipantsDto dto)
        {
            var leagueExists = await _db.Leagues
                .AsNoTracking()
                .AnyAsync(l => l.Id == LeagueId);

            if(!leagueExists)
            {
                return NotFound($"League with ID {LeagueId} not found.");
            }

            var participant = new LeagueParticipant
            {
                LeagueId = LeagueId,
                TeamName= dto.TeamName.Trim()
            };

            _db.LeagueParticipants.Add(participant);
            await _db.SaveChangesAsync();

            return Created($"api/League/{LeagueId}/participants/{participant.Id}", participant);
        }

        [HttpPost("{LeagueId:int}/draft/start")]
        public async Task<ActionResult<Draft>> StartDraft(int LeagueId, StartDraftDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var draft = await _draftServices.StartDraftAsync(LeagueId, dto.PicksPerParticipant, cancellationToken);
                return Ok(draft);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}