using LibrarySystem.DTOs.BorrowerDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using LibrarySystem.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/borrowers")]
    public class BorrowerController(IGenericRepository<Borrower> repo, BorrowerService service) : ControllerBase
    {
        private readonly IGenericRepository<Borrower> _repo = repo;
        private readonly BorrowerService _service = service;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BorrowerDto>))]
        public async Task<IActionResult> GetBorrowers()
        {
            var Borrowers = await _service.GetBorrowers();
            return Ok(Borrowers);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> GetBorrower([FromRoute]int id)
        {
            var borrower = await _service.GetBorrower(id);
            if (borrower == null)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });

            return Ok(borrower);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CreateBorrower([FromBody]CreateBorrowerRequestDto borrowerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var borrower = await _service.CreateBorrower(borrowerDto.ToBorrowerFromCreateDto());
            return Ok(borrower);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> DeleteBorrower([FromRoute]int id)
        {
            var isExists = await _service.DeleteBorrower(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> UpdateBorrower([FromRoute]int id,[FromBody]UpdateBorrowerRequestDto borrowerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var borrower = await _service.UpdateBorrower(id, borrowerDto);
            if (borrower == null)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });

            return Ok(borrower);
        }
    }
}
