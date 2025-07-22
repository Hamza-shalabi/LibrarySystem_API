using LibrarySystem.DTOs.LoanDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using LibrarySystem.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController(IGenericRepository<Loan> repo, LoanService service): ControllerBase
    {
        private readonly IGenericRepository<Loan> _repo = repo;
        private readonly LoanService _service = service;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanIncludeBorrowerBookDto>))]
        public async Task<IActionResult> GetLoans()
        {
            var Loans = await _service.GetLoans();
            return Ok(Loans);
        }

        [Authorize]
        [HttpGet("borrower/{Borrower_Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanBookIncludedDto>))]
        public async Task<IActionResult> GetLoansByBorrowerId([FromRoute]int Borrower_Id)
        {
            var Loans = await _service.GetLoansByBorrowerId(Borrower_Id);
            return Ok(Loans);
        }

        [Authorize]
        [HttpGet("book/{Book_Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanDto>))]
        public async Task<IActionResult> GetLoansByBookId([FromRoute] int Book_Id)
        {
            var Loans = await _service.GetLoansByBookId(Book_Id);
            return Ok(Loans);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoanIncludeBorrowerBookDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CreateLoan([FromBody]CreateLoanRequestDto loanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var loan = await _service.CreateLoan(loanDto.ToLoanFromCreateDto());
            return Ok(loan);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoanIncludeBorrowerBookDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> UpdateLoan([FromRoute] int id, [FromBody]UpdateLoanRequestDto loanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var loan = await _service.UpdateLoan(id, loanDto);
            if (loan == null)
                return NotFound(new Error { ErrorMessage = $"There is No Loan With This ID {id}" });

            return Ok(loan);
        }
    }
}
