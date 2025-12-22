using FilmsService.Data;
using FilmsService.DTOs;
using FilmsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public FilmsController(FilmDbContext context)
        {
            _context = context;
        }

        // --- GESTION DES FILMS ---

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null) return NotFound();
            return film;
        }

        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(CreateFilmDto filmDto)
        {
            var film = new Film
            {
                Titre = filmDto.Titre,
                Description = filmDto.Description,
                DureeMinutes = filmDto.DureeMinutes,
                Genre = filmDto.Genre,
                DateSortie = filmDto.DateSortie
            };

            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }

        // --- GESTION DES SÉANCES ---

        [HttpGet("seances")]
        public async Task<ActionResult<IEnumerable<Seance>>> GetSeances()
        {
            return await _context.Seances.Include(s => s.Tarifs).Include(s => s.Film).ToListAsync();
        }

        [HttpGet("seances/{id}")]
        public async Task<ActionResult<Seance>> GetSeance(int id)
        {
            var seance = await _context.Seances
                .Include(s => s.Tarifs)
                .Include(s => s.Film) // Optionnel, mais utile pour le debug
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seance == null) return NotFound();
            return seance;
        }

        [HttpPost("seances")]
        public async Task<ActionResult<Seance>> PostSeance(CreateSeanceDto seanceDto)
        {
            var seance = new Seance
            {
                FilmId = seanceDto.FilmId,
                SalleId = seanceDto.SalleId,
                DateHeure = seanceDto.DateHeure,
                Prix = seanceDto.Prix
            };

            _context.Seances.Add(seance);
            await _context.SaveChangesAsync(); // Sauvegarde pour avoir l'ID

            if (seanceDto.Tarifs != null && seanceDto.Tarifs.Any())
            {
                foreach (var tarifDto in seanceDto.Tarifs)
                {
                    _context.TarifsSeance.Add(new TarifSeance
                    {
                        SeanceId = seance.Id,
                        CategorieAge = tarifDto.CategorieAge,
                        Prix = tarifDto.Prix
                    });
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetSeance), new { id = seance.Id }, seance);
        }
    }
}

