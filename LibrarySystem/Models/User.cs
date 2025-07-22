using System.ComponentModel.DataAnnotations;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class User: IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public int Phone { get; set; }
        public string Role { get; set; } = "User";
    }
}
