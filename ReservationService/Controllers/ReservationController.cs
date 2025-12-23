using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.DTOs;
using ReservationService.DTOs.External;
using ReservationService.Models;
using System.Text;
using System.Text.Json;

namespace ReservationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // On sécurise l'ensemble du contrôleur
    public class ReservationController : ControllerBase
    {
        private readonly ReservationDbContext _context;
        private readonly HttpClient _httpClient;

        // URLs des autres services (à mettre idéalement dans appsettings)
        private const string FilmsServiceUrl = "http://localhost:5253/api";
        private const string SalleServiceUrl = "http://localhost:5035/api";
        private const string PaiementServiceUrl = "http://localhost:5112/api";

        public ReservationController(ReservationDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationBillets)
                .Include(r => r.ReservationSieges)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null) return NotFound();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Optionnel : Vérifier que l'utilisateur est bien le propriétaire de la réservation
            // if (reservation.UserId != userId) return Forbid();

            return Ok(reservation);
        }

        [HttpPut("{id}/annuler")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // if (reservation.UserId != userId) return Forbid();

            reservation.Statut = "Annulée";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 1. Récupérer les infos de la séance (Prix, etc.)
            // Appel au FilmsService pour avoir les détails de la séance et les tarifs
            var seanceResponse = await _httpClient.GetAsync($"{FilmsServiceUrl}/films/seances/{request.SeanceId}");
            if (!seanceResponse.IsSuccessStatusCode)
                return BadRequest("Séance introuvable.");

            var seanceContent = await seanceResponse.Content.ReadAsStringAsync();
            var seance = JsonSerializer.Deserialize<SeanceDto>(seanceContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (seance == null) return BadRequest("Erreur lors de la récupération de la séance.");

            // 2. Vérifier la disponibilité des sièges (SalleService)
            foreach (var siegeId in request.SiegeIds)
            {
                var siegeResponse = await _httpClient.GetAsync($"{SalleServiceUrl}/salle/sieges/{siegeId}");
                if (!siegeResponse.IsSuccessStatusCode)
                    return BadRequest($"Siège {siegeId} introuvable.");
                
                // TODO: Vérifier si le siège est déjà réservé pour cette séance (nécessite une logique dans SalleService ou ReservationService)
                // Pour l'instant, on suppose qu'ils sont libres si on peut les récupérer.
            }

            // 3. Calculer le montant total
            decimal total = 0;
            
            // Calculer prix des billets selon l'âge
            foreach (var billet in request.Billets)
            {
                // Chercher le tarif correspondant dans la séance
                var tarif = seance.Tarifs.FirstOrDefault(t => t.CategorieAge == (int)billet.CategorieAge);
                decimal prixUnitaire = tarif != null ? tarif.Prix : seance.Prix; // Prix spécifique ou prix par défaut

                total += prixUnitaire * billet.Quantite;
            }

            // Ajouter le prix des sièges (si les sièges sont payants en plus, sinon inclus)
            // Ici on suppose que le prix dépend uniquement du type de billet (enfant/adulte)

            // 4. Créer la réservation "En Cours"
            var reservation = new Reservation
            {
                UserId = userId,
                SeanceId = request.SeanceId,
                DateCreation = DateTime.UtcNow,
                Statut = "EnCours",
                Total = total
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Sauvegarder les détails (Billets)
            foreach (var b in request.Billets)
            {
                // Recalculer prix unitaire pour stockage
                var tarif = seance.Tarifs.FirstOrDefault(t => t.CategorieAge == (int)b.CategorieAge);
                decimal prixUnitaire = tarif != null ? tarif.Prix : seance.Prix;

                _context.ReservationBillets.Add(new ReservationBillet
                {
                    ReservationId = reservation.ReservationId,
                    CategorieAge = b.CategorieAge,
                    Quantite = b.Quantite,
                    PrixUnitaire = prixUnitaire
                });
            }

            // Sauvegarder les sièges
            foreach (var siegeId in request.SiegeIds)
            {
                _context.ReservationSieges.Add(new ReservationSiege
                {
                    ReservationId = reservation.ReservationId,
                    SiegeId = siegeId
                });
            }
            await _context.SaveChangesAsync();

            // 5. Appeler le service de Paiement
            var paiementRequest = new
            {
                ReservationId = reservation.ReservationId,
                Montant = total,
                Devise = "cad"
            };

            var jsonPaiement = new StringContent(JsonSerializer.Serialize(paiementRequest), Encoding.UTF8, "application/json");
            var paiementResponse = await _httpClient.PostAsync($"{PaiementServiceUrl}/paiement", jsonPaiement);

            if (paiementResponse.IsSuccessStatusCode)
            {
                var paiementContent = await paiementResponse.Content.ReadAsStringAsync();
                var paiementInfo = JsonSerializer.Deserialize<PaiementResponseDto>(paiementContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return Ok(new 
                { 
                    ReservationId = reservation.ReservationId, 
                    Total = total,
                    Message = "Réservation créée, paiement initié.",
                    PaiementInfo = paiementInfo
                });
            }
            else
            {
                return StatusCode(500, "Erreur lors de l'initiation du paiement.");
            }
        }
    }
}
