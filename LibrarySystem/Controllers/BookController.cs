using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController(IGenericRepository<Book> repo) : ControllerBase
    {
        private readonly IGenericRepository<Book> _repo = repo;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BookDto>))]
        public async Task<IActionResult> GetBooks()
        {
            var Books = await _repo.GetAllAsync(x => x.Author);
            
            var BooksDto = Books.Select(s => s.ToBookDto());
            return Ok(BooksDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDto))]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Book With This ID {id}" });

            var book = await _repo.GetAsync(id, (x => x.Author));

            var BookDto = book.ToBookDto(); 
            return Ok(BookDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDto))]
        public async Task<IActionResult> CreateBook([FromBody]CreateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var book = bookDto.ToBookFromCreateDto();
            await _repo.CreateAsync(book);
            return Ok(book.ToBookDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            await _repo.DeleteAsync(id);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDto))]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody]UpdateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            if (bookDto.Id != id)
                return BadRequest(new Error { ErrorMessage = "ID's Mismatch" });

            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Book With This ID {id}" });

            var BookTU = await _repo.GetAsync(id);
            BookTU.Id = bookDto.Id;
            BookTU.Title = bookDto.Title;
            BookTU.ISBN = bookDto.ISBN;
            BookTU.PublishedDate = bookDto.PublishedDate;
            BookTU.LastUpdatedAt = DateTime.Now.ToUniversalTime();

            var Book = await _repo.UpdateAsync(BookTU);
            return Ok(Book.ToBookDto());
        }
    }
}
