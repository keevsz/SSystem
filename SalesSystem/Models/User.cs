using System.ComponentModel.DataAnnotations;
using static SalesSystem.Models.Roles;

namespace SalesSystem.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Gender { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public RoleType Role { get; set; } = RoleType.USER;

    }
}