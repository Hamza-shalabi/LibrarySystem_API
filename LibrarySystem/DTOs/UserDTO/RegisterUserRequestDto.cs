using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.UserDTO
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public int Phone { get; set; }
    }
}
