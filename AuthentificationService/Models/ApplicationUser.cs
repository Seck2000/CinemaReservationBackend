using Microsoft.AspNetCore.Identity;

namespace AuthentificationService.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ajoutez ici des propriétés personnalisées si nécessaire
        public string? FullName { get; set; }
    }
}

