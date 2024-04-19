using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzczurApp.Data.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
