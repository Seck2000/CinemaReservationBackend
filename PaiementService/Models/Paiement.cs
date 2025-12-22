namespace PaiementService.Models
{
    public class Paiement
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal Montant { get; set; }
        public string Statut { get; set; } = "EnAttente"; // EnAttente, Reussi, Echec
        public string? StripePaymentIntentId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}

