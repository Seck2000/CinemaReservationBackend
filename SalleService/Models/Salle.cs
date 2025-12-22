namespace SalleService.Models
{
    public class Salle
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Capacite { get; set; }

        public List<Siege> Sieges { get; set; } = new();
    }
}

