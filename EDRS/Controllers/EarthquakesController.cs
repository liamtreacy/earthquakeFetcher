using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EarthquakeRetrieverService.Models;
using ExternalDataRetrieverService.Models;
using Microsoft.IdentityModel.Tokens;

namespace ExternalDataRetrieverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarthquakesController : ControllerBase
    {
        private readonly EarthquakeContext _context;

        public EarthquakesController(EarthquakeContext context)
        {
            _context = context;
        }

        // GET: api/Earthquakes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Earthquake>>> GetEarthquakes()
        {
          if (_context.Earthquakes == null)
          {
              return NotFound();
          }
          
          var service = new ExternalDataRetrieverService.Services.
              EarthquakeRetrieverService();
          await service.Get(_context);
          
          return await _context.Earthquakes.ToListAsync();
        }
        
        // GET: api/Earthquakes/LargestMag
        [HttpGet("LargestMag")]
        public async Task<ActionResult<Earthquake>> GetLargestMag()
        {
            if (_context.Earthquakes.IsNullOrEmpty())
            {
                return NotFound();
            }

            return await _context.Earthquakes.FirstAsync();
        }

        // GET: api/Earthquakes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Earthquake>> GetEarthquake(string id)
        {
          if (_context.Earthquakes == null)
          {
              return NotFound();
          }
            var earthquake = await _context.Earthquakes.FindAsync(id);

            if (earthquake == null)
            {
                return NotFound();
            }

            return earthquake;
        }

        // DELETE: api/Earthquakes/
        [HttpDelete]
        public async Task<IActionResult> DeleteEarthquakes()
        {
            if (_context.Earthquakes == null)
            {
                return NotFound();
            }

            var earthquakes = await _context.Earthquakes.ToListAsync();
            if (earthquakes == null)
            {
                return NotFound();
            }

            _context.Earthquakes.RemoveRange(earthquakes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
