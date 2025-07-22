using System.ComponentModel.DataAnnotations;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class User: IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string Role { get; set; } = "User";
    }
}
