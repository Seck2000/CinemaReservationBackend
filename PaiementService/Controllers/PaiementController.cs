using Microsoft.AspNetCore.Mvc;
using PaiementService.Data;
using PaiementService.DTOs;
using PaiementService.Models;
using Stripe;

namespace PaiementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementController : ControllerBase
    {
        private readonly PaiementDbContext _context;
        private readonly IConfiguration _configuration;

        public PaiementController(PaiementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaiementRequestDto request)
        {
            // 1. Créer le PaymentIntent chez Stripe
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Montant * 100), // Stripe utilise les centimes
                Currency = request.Devise,
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "ReservationId", request.ReservationId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            PaymentIntent intent;

            try
            {
                intent = await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

            // 2. Enregistrer l'intention de paiement dans notre DB
            var paiement = new Paiement
            {
                ReservationId = request.ReservationId,
                Montant = request.Montant,
                Statut = "EnAttente",
                StripePaymentIntentId = intent.Id,
                Date = DateTime.UtcNow
            };

            _context.Paiements.Add(paiement);
            await _context.SaveChangesAsync();

            // 3. Retourner le ClientSecret au frontend (pour finaliser le paiement côté client)
            return Ok(new
            {
                PaiementId = paiement.Id,
                ClientSecret = intent.ClientSecret,
                StripePaymentIntentId = intent.Id
            });
        }
    }
}

