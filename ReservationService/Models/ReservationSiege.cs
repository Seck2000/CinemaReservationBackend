namespace ReservationService.Models
{
    public class ReservationSiege
    {
        public int ReservationSiegeId { get; set; }
        public int ReservationId { get; set; }
        public int SiegeId { get; set; } // vient du SalleService
        public Reservation? Reservation { get; set; }
    }
}

