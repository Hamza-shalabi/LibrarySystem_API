using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.UserDTO
{
    public class UpdateUserRequestDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public int Phone { get; set; }
        public string Role { get; set; } = "User";
    }
}
