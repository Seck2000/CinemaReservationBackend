using System.ComponentModel.DataAnnotations;

namespace AuthentificationService.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? FullName { get; set; }

        // Rôle par défaut : "Client" si non spécifié
        public string Role { get; set; } = "Client";
    }
}

