using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.BookDTO
{
    public class CreateBookRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public long ISBN { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public int Author_Id { get; set; }
    }
}
