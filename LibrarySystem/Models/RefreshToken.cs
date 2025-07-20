using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
