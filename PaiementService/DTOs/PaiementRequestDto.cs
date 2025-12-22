namespace PaiementService.DTOs
{
    public class PaiementRequestDto
    {
        public int ReservationId { get; set; }
        public decimal Montant { get; set; }
        public string Devise { get; set; } = "cad"; // Par défaut dollars canadiens
    }
}

