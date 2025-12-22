using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalleService.Data;
using SalleService.DTOs;
using SalleService.Models;

namespace SalleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalleController : ControllerBase
    {
        private readonly SalleDbContext _context;

        public SalleController(SalleDbContext context)
        {
            _context = context;
        }

        // --- GESTION DES SALLES ---

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salle>>> GetSalles()
        {
            return await _context.Salles.Include(s => s.Sieges).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Salle>> GetSalle(int id)
        {
            var salle = await _context.Salles.Include(s => s.Sieges).FirstOrDefaultAsync(s => s.Id == id);
            if (salle == null) return NotFound();
            return salle;
        }

        [HttpPost]
        public async Task<ActionResult<Salle>> PostSalle(CreateSalleDto salleDto)
        {
            var salle = new Salle
            {
                Nom = salleDto.Nom,
                Capacite = salleDto.Capacite
            };

            _context.Salles.Add(salle);
            await _context.SaveChangesAsync();

            // Création automatique de sièges pour faciliter le test ?
            // Optionnel : On peut créer X sièges par défaut
            for (int i = 1; i <= salle.Capacite; i++)
            {
                 _context.Sieges.Add(new Siege
                 {
                     Numero = $"S{i}",
                     SalleId = salle.Id
                 });
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalle), new { id = salle.Id }, salle);
        }

        // --- GESTION DES SIÈGES ---

        [HttpGet("sieges/{id}")]
        public async Task<ActionResult<Siege>> GetSiege(int id)
        {
            var siege = await _context.Sieges.FindAsync(id);
            if (siege == null) return NotFound();
            return siege;
        }
        
        [HttpGet("sieges")]
        public async Task<ActionResult<IEnumerable<Siege>>> GetSieges()
        {
            return await _context.Sieges.ToListAsync();
        }

        [HttpPost("sieges")]
        public async Task<ActionResult<Siege>> PostSiege(CreateSiegeDto siegeDto)
        {
            var siege = new Siege
            {
                Numero = siegeDto.Numero,
                SalleId = siegeDto.SalleId
            };

            _context.Sieges.Add(siege);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSiege), new { id = siege.Id }, siege);
        }
    }
}

