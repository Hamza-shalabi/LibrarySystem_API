using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.Models;

namespace LibrarySystem.DTOs.AuthorDTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public List<BookIncludeDto> Books { get; set; }
    }
}
