using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FilmsService.Models
{
    public class TarifSeance
    {
        public int TarifSeanceId { get; set; }
        public int SeanceId { get; set; }
        public CategorieAge CategorieAge { get; set; }

        [Range(0, 9999)]
        public decimal Prix { get; set; }

        [JsonIgnore]
        public Seance? Seance { get; set; }
    }
}

