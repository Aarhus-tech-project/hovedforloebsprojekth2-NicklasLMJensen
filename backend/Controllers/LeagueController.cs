using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaDraftLeague.Backend.Data;
using MediaDraftLeague.Backend.Domain.Entities;
using MediaDraftLeague.Backend.Dtos;
using System.Runtime.Versioning;


namespace MediaDraftLeague.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LeagueController : ControllerBase
    {
        private readonly AppDbContext _db;
        public LeagueController(AppDbContext db)
        {
            _db = db;
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
    }
}