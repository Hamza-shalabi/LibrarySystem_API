using LibrarySystem.DTOs.AuthorDTO;
using LibrarySystem.Facade;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController(AuthorService service) : ControllerBase
    {
        private readonly AuthorService _service = service;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<AuthorDto>))]
        public async Task<IActionResult> GetAuthors()
        {
            var Authors= await _service.GetAuthors();

            return Ok(Authors);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var author = await _service.GetAuthor(id);
            if(author == null)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            return Ok(author);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> CreateAuthor([FromBody]CreateAuthorRequestDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var Author = await _service.CreateAuthor(authorDto.ToAuthorFromCreateDto());
            return Ok(Author);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var isExists = await _service.DeleteAuthor(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody]UpdateAuthorRequestDto authorDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var author = await _service.UpdateAuthor(id, authorDto);
            if (author == null)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            return Ok(author);
        }
    }
}
