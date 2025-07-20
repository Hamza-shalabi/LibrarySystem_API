using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.UserDTO
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
