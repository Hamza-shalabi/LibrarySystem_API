using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.AuthorDTO;

namespace LibrarySystem.DTOs.BookDTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public long ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
        public AuthorIncludeDto Author { get; set; }
    }
}
