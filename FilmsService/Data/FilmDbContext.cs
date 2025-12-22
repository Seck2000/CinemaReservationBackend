using Microsoft.EntityFrameworkCore;
using FilmsService.Models;

namespace FilmsService.Data
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<TarifSeance> TarifsSeance => Set<TarifSeance>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TarifSeance>()
                .HasKey(t => t.TarifSeanceId);

            modelBuilder.Entity<TarifSeance>()
                .HasOne(t => t.Seance)
                .WithMany(s => s.Tarifs)
                .HasForeignKey(t => t.SeanceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

