using System.Text.Json.Serialization;

namespace SalleService.Models
{
    public class Siege
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty; // Ex: "A1", "B5"
        public int SalleId { get; set; }

        [JsonIgnore]
        public Salle? Salle { get; set; }
    }
}

