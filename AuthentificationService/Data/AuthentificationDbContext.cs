using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthentificationService.Models;

namespace AuthentificationService.Data
{
    public class AuthentificationDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthentificationDbContext(DbContextOptions<AuthentificationDbContext> options)
            : base(options)
        {
        }
    }
}

