using Microsoft.EntityFrameworkCore;
using SalleService.Models;

namespace SalleService.Data
{
    public class SalleDbContext : DbContext
    {
        public SalleDbContext(DbContextOptions<SalleDbContext> options) : base(options)
        {
        }

        public DbSet<Salle> Salles { get; set; }
        public DbSet<Siege> Sieges { get; set; }
    }
}

