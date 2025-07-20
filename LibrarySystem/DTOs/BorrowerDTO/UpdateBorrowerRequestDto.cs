using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.BorrowerDTO
{
    public class UpdateBorrowerRequestDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public long Phone { get; set; }
    }
}
