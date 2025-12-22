namespace ReservationService.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int SeanceId { get; set; } // vient du CatalogueService
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public string Statut { get; set; } = "EnCours";
        public decimal Total { get; set; }
    }
}

