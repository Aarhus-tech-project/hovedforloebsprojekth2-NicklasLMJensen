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
    public class MediaItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MediaItemsController(AppDbContext db)
        {
            _db = db;
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaItem>>> GetAll()
        {
            var items = await _db.MediaItems
                .AsNoTracking()
                .ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<MediaItem>> Create(MediaItemDto dto)
        {
            var item = new MediaItem
            {
                Title = dto.Title,
                Type = dto.Type,
                Score = dto.Score
            };

            _db.MediaItems.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = item.Id }, item);
        }
    }
}
