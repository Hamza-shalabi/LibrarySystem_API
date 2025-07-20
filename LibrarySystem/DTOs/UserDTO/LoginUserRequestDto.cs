using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.UserDTO
{
    public class LoginUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
