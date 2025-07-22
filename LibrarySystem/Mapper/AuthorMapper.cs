using LibrarySystem.DTOs.AuthorDTO;
using LibrarySystem.Models;

namespace LibrarySystem.Mapper
{
    public static class AuthorMapper
    {
        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio,
                Books = author.Books.Select(s => s.ToBookIncludeDto()).ToList(),
            };
        }
        public static AuthorIncludeDto ToAuthorIncludeDto(this Author author)
        {
            return new AuthorIncludeDto
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio,
            };
        }

        public static Author ToAuthorFromCreateDto(this CreateAuthorRequestDto author)
        {
            return new Author
            {
                Name = author.Name,
                Bio = author.Bio,
            };
        }

        public static Author ToAuthorFromUpdateDto(this UpdateAuthorRequestDto author)
        {
            return new Author
            {
                Name = author.Name,
                Bio = author.Bio,
                LastUpdatedAt = DateTime.Now.ToUniversalTime(),
            };
        }
    }
}
