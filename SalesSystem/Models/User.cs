using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public string Password { get; set; }= null!;

        [Required]
        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public Role? Role { get; set; }
    }

    public class UserCreateDTO
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
        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public Role? Role { get; set; }
    }

    public class UserUpdateDTO
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

        [AllowNull]
        [MaxLength(100)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{12,}$",
            ErrorMessage = "The password must be at least 12 characters long and contain " +
            "at least one lowercase letter, one uppercase letter, one digit, " +
            "and one special character.")]
        public string Password { get; set; } = null!;

        [Required]
        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public Role? Role { get; set; }
    }
}