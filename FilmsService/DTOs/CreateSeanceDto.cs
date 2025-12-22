using FilmsService.Models;

namespace FilmsService.DTOs
{
    public class CreateSeanceDto
    {
        public int FilmId { get; set; }
        public int SalleId { get; set; }
        public DateTime DateHeure { get; set; }
        public decimal Prix { get; set; }
        public List<CreateTarifDto> Tarifs { get; set; } = new();
    }

    public class CreateTarifDto
    {
        public CategorieAge CategorieAge { get; set; }
        public decimal Prix { get; set; }
    }
}

