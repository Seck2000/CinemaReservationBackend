using ReservationService.Models;

namespace ReservationService.DTOs
{
    public class CreateReservationDto
    {
        public int SeanceId { get; set; }
        public List<int> SiegeIds { get; set; } = new();
        public List<ReservationBilletDto> Billets { get; set; } = new();
    }

    public class ReservationBilletDto
    {
        public CategorieAge CategorieAge { get; set; }
        public int Quantite { get; set; }
    }
}
