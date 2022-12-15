using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EarthquakeRetrieverService.Models;
using ExternalDataRetrieverService.Models;

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

        // PUT: api/Earthquakes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarthquake(string id, Earthquake earthquake)
        {
            if (id != earthquake.Id)
            {
                return BadRequest();
            }

            _context.Entry(earthquake).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EarthquakeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Earthquakes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Earthquake>> PostEarthquake(Earthquake earthquake)
        {
          if (_context.Earthquakes == null)
          {
              return Problem("Entity set 'EarthquakeContext.Earthquakes'  is null.");
          }
            _context.Earthquakes.Add(earthquake);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EarthquakeExists(earthquake.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEarthquake", new { id = earthquake.Id }, earthquake);
        }

        // DELETE: api/Earthquakes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEarthquake(string id)
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

            _context.Earthquakes.Remove(earthquake);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EarthquakeExists(string id)
        {
            return (_context.Earthquakes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
