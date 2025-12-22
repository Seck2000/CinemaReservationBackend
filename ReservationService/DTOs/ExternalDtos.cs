namespace ReservationService.DTOs
{
    // Réponse de SalleService
    public class SiegeDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public bool EstLibre { get; set; } = true; // Simplification pour le TP
    }

    // Réponse de PaiementService
    public class PaiementResponseDto
    {
        public int PaiementId { get; set; }
        public string ClientSecret { get; set; } = string.Empty;
        public string StripePaymentIntentId { get; set; } = string.Empty;
    }

    // Requête vers PaiementService
    public class PaiementRequestDto
    {
        public int ReservationId { get; set; }
        public decimal Montant { get; set; }
        public string Devise { get; set; } = "cad";
    }
}

