using LibrarySystem.DTOs.LoanDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController(IGenericRepository<Loan> repo): ControllerBase
    {
        private readonly IGenericRepository<Loan> _repo = repo;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanDto>))]
        public async Task<IActionResult> GetLoans()
        {
            var Loans = await _repo.GetAllAsync([x => x.Book, x => x.Borrower]);

            var LoansDto = Loans.Select(l => l.ToLoanDto());
            return Ok(LoansDto);
        }

        [Authorize]
        [HttpGet("borrower/{Borrower_Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanBookIncludedDto>))]
        public async Task<IActionResult> GetLoansByBorrowerId([FromRoute]int Borrower_Id)
        {
            var Loans = await _repo.GetByValueAsync(x => x.Borrower_Id == Borrower_Id, x =>x.Book);
            var LoansDto = Loans.Select(s => s.ToLoanBookIncludedDto());
            return Ok(LoansDto);
        }

        [Authorize]
        [HttpGet("book/{Book_Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoanBorrowerIncludedDto>))]
        public async Task<IActionResult> GetLoansByBookId([FromRoute] int Book_Id)
        {
            var Loans = await _repo.GetByValueAsync(x => x.Book_Id == Book_Id, x => x.Borrower);
            var LoansDto = Loans.Select(s => s.ToLoanBorrowerIncludedDto());
            return Ok(LoansDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoanDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        public async Task<IActionResult> CreateLoan([FromBody]CreateLoanRequestDto loanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements {ModelState}" });

            var loan = loanDto.ToLoanFromCreateDto();
            await _repo.CreateAsync(loan);
            return Ok(loan.ToLoanDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoanDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<IActionResult> UpdateLoan([FromRoute] int id, [FromBody]UpdateLoanRequestDto loanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            if (loanDto.Id != id)
                return BadRequest(new Error { ErrorMessage = "ID's Mismatch" });

            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return NotFound(new Error { ErrorMessage = $"There is No Author With This ID {id}" });

            var originalLoan = await _repo.GetAsync(id);
            originalLoan.LoanDate = loanDto.LoanDate;
            originalLoan.ReturnDate = loanDto.ReturnDate;

            var loan = await _repo.UpdateAsync(originalLoan);
            return Ok(loan.ToLoanDto());
        }
    }
}
