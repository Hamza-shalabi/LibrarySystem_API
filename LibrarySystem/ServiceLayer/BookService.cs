using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;

namespace LibrarySystem.ServiceLayer
{
    public class BookService(IGenericRepository<Book> repo)
    {
        private readonly IGenericRepository<Book> _repo = repo;

        public async Task<List<BookDto>> GetBooks()
        {
            var books = await _repo.GetAllAsync(x => x.Author);
            var booksDto = books.Select(s => s.ToBookDto()).ToList();
            return booksDto;
        }
        public async Task<BookDto?> GetBook(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var book = await _repo.GetAsync(id, x => x.Author);
            return book?.ToBookDto();
        }
        public async Task<BookCUDto> CreateBook(Book book)
        {
            await _repo.CreateAsync(book);
            return book.ToBookCUDto();
        }
        public async Task<bool> DeleteBook(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<BookCUDto?> UpdateBook(int id, UpdateBookRequestDto bookDto)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var originalBook = await _repo.GetAsync(id);
            originalBook.Title = bookDto.Title;
            originalBook.ISBN = bookDto.ISBN;
            originalBook.PublishedDate = bookDto.PublishedDate;
            originalBook.LastUpdatedAt = DateTime.Now.ToUniversalTime();

            var updatedBook = await _repo.UpdateAsync(originalBook);
            return updatedBook?.ToBookCUDto();
        }
    }
}
