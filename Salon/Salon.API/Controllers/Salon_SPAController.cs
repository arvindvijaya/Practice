using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon.Data.Models;

namespace Salon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Salon_SPAController : ControllerBase
    {
        private readonly SalonContext _context;

        public Salon_SPAController(SalonContext context)
        {
            _context = context;

            if ( _context.Salons.Count()==0)
            {
                _context.Salons.Add(new Salon_SPA { Id = 1, Name = "Majestic", Email = "baba@majestic.com" });
                _context.SaveChanges();
            }
        }

        // GET: api/Salon_SPA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salon_SPA>>> GetSalons()
        {
            return await _context.Salons.ToListAsync();
        }

        // GET: api/Salon_SPA/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salon_SPA>> GetSalon_SPA(long id)
        {
            var salon_SPA = await _context.Salons.FindAsync(id);

            if (salon_SPA == null)
            {
                return NotFound();
            }

            return salon_SPA;
        }

        // PUT: api/Salon_SPA/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalon_SPA(long id, Salon_SPA salon_SPA)
        {
            if (id != salon_SPA.Id)
            {
                return BadRequest();
            }

            _context.Entry(salon_SPA).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Salon_SPAExists(id))
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

        // POST: api/Salon_SPA
        [HttpPost]
        public async Task<ActionResult<Salon_SPA>> PostSalon_SPA(Salon_SPA salon_SPA)
        {
            _context.Salons.Add(salon_SPA);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalon_SPA", new { id = salon_SPA.Id }, salon_SPA);
        }

        // DELETE: api/Salon_SPA/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Salon_SPA>> DeleteSalon_SPA(long id)
        {
            var salon_SPA = await _context.Salons.FindAsync(id);
            if (salon_SPA == null)
            {
                return NotFound();
            }

            _context.Salons.Remove(salon_SPA);
            await _context.SaveChangesAsync();

            return salon_SPA;
        }

        private bool Salon_SPAExists(long id)
        {
            return _context.Salons.Any(e => e.Id == id);
        }
    }
}
