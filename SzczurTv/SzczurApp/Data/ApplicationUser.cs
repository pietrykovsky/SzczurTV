using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SzczurApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public override string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public override string Email { get; set; } = string.Empty;

        // Additional properties can be added here if needed
        public string? StreamKey { get; set; }
        public string? StreamTitle { get; set; }
        public string? StreamCategory { get; set; }
        public string? Bio { get; set; }
    }
}
