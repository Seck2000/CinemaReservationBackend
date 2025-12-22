using System.Text.Json.Serialization;

namespace FilmsService.Models
{
    public class Seance
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int SalleId { get; set; }
        public DateTime DateHeure { get; set; }
        public decimal Prix { get; set; }

        [JsonIgnore]
        public Film? Film { get; set; }

        public List<TarifSeance> Tarifs { get; set; } = new();
    }
}

