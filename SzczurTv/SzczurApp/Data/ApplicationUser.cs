using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
    }
}
