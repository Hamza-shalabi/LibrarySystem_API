using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using LibrarySystem.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController(IGenericRepository<Book> repo, BookService serivce) : ControllerBase
    {
        private readonly IGenericRepository<Book> _repo = repo;
        private readonly BookService _service = serivce;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BookDto>))]
        public async Task<IActionResult> GetBooks()
        {
            var Books = await _service.GetBooks();
            return Ok(Books);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDto))]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            var Book = await _service.GetBook(id);
            if (Book == null)
                return NotFound(new Error { ErrorMessage = $"There is No Book With This ID {id}" });

            return Ok(Book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookCUDto))]
        public async Task<IActionResult> CreateBook([FromBody]CreateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var book = await _service.CreateBook(bookDto.ToBookFromCreateDto());
            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var isExists = await _service.DeleteBook(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Book With This ID {id}" });

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookCUDto))]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody]UpdateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var book = await _service.UpdateBook(id, bookDto);
            if (book == null)
                return NotFound(new Error { ErrorMessage = $"There is No Book With This ID {id}" });

            return Ok(book);
        }
    }
}
