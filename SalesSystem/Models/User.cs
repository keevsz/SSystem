using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SalesSystem.Models.Roles;

namespace SalesSystem.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(120)]
        public string FirstName { get; set; } = null!;
        
        [Required]
        [MinLength(1)]
        [MaxLength(120)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [RegularExpression("^[MFN]$", ErrorMessage = "Gender must be 'M', 'F', or 'N'.")]
        public string Gender { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{12,}$",
            ErrorMessage = "The password must be at least 12 characters long and contain " +
            "at least one lowercase letter, one uppercase letter, one digit, " +
            "and one special character.")]
        public string Password { get; set; } = null!;

        [Required]
        public RoleType Role { get; set; } = RoleType.USER;

    }
}