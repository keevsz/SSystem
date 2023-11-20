using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

    }
}
