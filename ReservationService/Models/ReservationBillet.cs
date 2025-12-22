namespace ReservationService.Models
{
    public class ReservationBillet
    {
        public int ReservationBilletId { get; set; }
        public int ReservationId { get; set; }
        public CategorieAge CategorieAge { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; } // fixé au moment de réserver
        public Reservation? Reservation { get; set; }
    }
}

