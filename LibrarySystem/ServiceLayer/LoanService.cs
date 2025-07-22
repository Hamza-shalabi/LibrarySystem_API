using LibrarySystem.DTOs.LoanDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;

namespace LibrarySystem.ServiceLayer
{
    public class LoanService(IGenericRepository<Loan> repo)
    {
        private readonly IGenericRepository<Loan> _repo = repo;

        public async Task<List<LoanIncludeBorrowerBookDto>?> GetLoans()
        {
            var Loans = await _repo.GetAllAsync([x => x.Book, x => x.Borrower]);

            var LoansDto = Loans.Select(l => l.ToLoanIncludeBorrowerBookDto()).ToList();
            return LoansDto;
        }
        public async Task<List<LoanBookIncludedDto>?> GetLoansByBorrowerId(int Borrower_Id)
        {
            var Loans = await _repo.GetByValueAsync(x => x.Borrower_Id == Borrower_Id, x => x.Book);
            var LoansDto = Loans.Select(s => s.ToLoanBookIncludedDto()).ToList();
            return LoansDto;
        }
        public async Task<List<LoanBorrowerIncludedDto>?> GetLoansByBookId(int Book_Id)
        {
            var Loans = await _repo.GetByValueAsync(x => x.Book_Id == Book_Id, x => x.Borrower);
            var LoansDto = Loans.Select(s => s.ToLoanBorrowerIncludedDto()).ToList();
            return LoansDto;
        }
        public async Task<LoanDto> CreateLoan(Loan loan)
        {
            await _repo.CreateAsync(loan);
            return loan.ToLoanDto();
        }
        public async Task<LoanDto?> UpdateLoan(int id, UpdateLoanRequestDto loanDto)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var originalLoan = await _repo.GetAsync(id);
            originalLoan.LoanDate = loanDto.LoanDate;
            originalLoan.ReturnDate = loanDto.ReturnDate;
            originalLoan.LastUpdatedAt = DateTime.Now.ToUniversalTime();

            var loan = await _repo.UpdateAsync(originalLoan);
            return loan.ToLoanDto();
        }
    }
}
