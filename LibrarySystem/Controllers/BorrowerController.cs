using LibrarySystem.DTOs.BorrowerDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/borrowers")]
    public class BorrowerController(IGenericRepository<Borrower> repo) : ControllerBase
    {
        private readonly IGenericRepository<Borrower> _repo = repo;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BorrowerDto>))]
        public async Task<IActionResult> GetBorrowers()
        {
            var Borrowers = await _repo.GetAllAsync();

            var BorrowersDto = Borrowers.Select(b => b.ToBorrowerDto());
            return Ok(BorrowersDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> GetBorrower([FromRoute]int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });

            var borrower = await _repo.GetAsync(id);
            var borrowerDto = borrower.ToBorrowerDto();
            return Ok(borrowerDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CreateBorrower([FromBody]CreateBorrowerRequestDto borrowerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var borrower = borrowerDto.ToBorrowerFromCreateDto();
            await _repo.CreateAsync(borrower);
            return Ok(borrower.ToBorrowerDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> DeleteBorrower([FromRoute]int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });

            var borrower = await _repo.DeleteAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BorrowerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> UpdateBorrower([FromRoute]int id,[FromBody]UpdateBorrowerRequestDto borrowerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            if (borrowerDto.Id != id)
                return BadRequest(new Error { ErrorMessage = "ID's Mismatch" });

            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Borrower With This ID {id}" });

            var originalBorrower = await _repo.GetAsync(id);
            originalBorrower.Id = borrowerDto.Id;
            originalBorrower.Name = borrowerDto.Name;
            originalBorrower.Email = borrowerDto.Email;
            originalBorrower.Phone = borrowerDto.Phone;

            var borrower = await _repo.UpdateAsync(originalBorrower);
            return Ok(borrower.ToBorrowerDto());
        }
    }
}
