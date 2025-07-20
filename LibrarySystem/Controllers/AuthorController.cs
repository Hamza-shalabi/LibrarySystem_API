using LibrarySystem.DTOs.AuthorDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController(IGenericRepository<Author> repo) : ControllerBase
    {
        private readonly IGenericRepository<Author> _repo = repo;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<AuthorDto>))]
        public async Task<IActionResult> GetAuthors()
        {
            var Authors= await _repo.GetAllAsync(x=>x.Books);

            var AuthorsDto = Authors.Select(s => s.ToAuthorDto());

            return Ok(AuthorsDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            var author = await _repo.GetAsync(id, (x => x.Books));
            return Ok(author.ToAuthorDto());
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> CreateAuthor([FromBody]CreateAuthorRequestDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var Author = authorDto.ToAuthorFromCreateDto();
            await _repo.CreateAsync(Author);
            return Ok(Author.ToAuthorDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            var author = await _repo.DeleteAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody]UpdateAuthorRequestDto authorDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            if(authorDto.Id != id)
                return BadRequest(new Error { ErrorMessage = "ID's Mismatch" });

            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            var originalAuthor = await _repo.GetAsync(id);
            originalAuthor.Id = authorDto.Id;
            originalAuthor.Name = authorDto.Name;
            originalAuthor.Bio = authorDto.Bio;

            var Author = await _repo.UpdateAsync(originalAuthor);
            return Ok(Author.ToAuthorDto());
        }
    }
}
