using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.Models;

namespace LibrarySystem.Mapper
{
    public static class BookMapper
    {
        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto
            {
                Id=book.Id,
                Title=book.Title,
                ISBN=book.ISBN,
                PublishedDate=book.PublishedDate,
                Author = book.Author.ToAuthorIncludeDto(),
            };
        }

        public static BookIncludeDto ToBookIncludeDto(this Book book)
        {
            return new BookIncludeDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,

            };
        }

        public static Book ToBookFromCreateDto(this CreateBookRequestDto book)
        {
            return new Book
            {
                Title=book.Title,
                ISBN=book.ISBN,
                PublishedDate=book.PublishedDate,
                Author_Id = book.Author_Id,
            };
        }

        public static Book ToBookFromUpdateDto(this UpdateBookRequestDto book)
        {
            return new Book
            {
                Id = book.Id,
                Title=book.Title,
                ISBN=book.ISBN,
                PublishedDate=book.PublishedDate
            };
        }
    }
}
