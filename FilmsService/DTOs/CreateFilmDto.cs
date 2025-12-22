namespace FilmsService.DTOs
{
    public class CreateFilmDto
    {
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DureeMinutes { get; set; }
        public string Genre { get; set; } = string.Empty;
        public DateTime DateSortie { get; set; }
    }
}

