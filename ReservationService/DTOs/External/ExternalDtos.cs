namespace ReservationService.DTOs.External
{
    public class SiegeDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int SalleId { get; set; }
    }

    public class PaiementResponseDto
    {
        public int PaiementId { get; set; }
        public string ClientSecret { get; set; } = string.Empty;
        public string StripePaymentIntentId { get; set; } = string.Empty;
    }

    public class FilmDto 
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
    }

    public class SeanceDto
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int SalleId { get; set; }
        public DateTime DateHeure { get; set; }
        public decimal Prix { get; set; }
        public List<TarifSeanceDto> Tarifs { get; set; } = new();
    }

    public class TarifSeanceDto
    {
        public int TarifSeanceId { get; set; }
        public int CategorieAge { get; set; } // int car l'enum est locale
        public decimal Prix { get; set; }
    }
}

