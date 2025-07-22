using LibrarySystem.DTOs.LoanDTO;
using LibrarySystem.Models;

namespace LibrarySystem.Mapper
{
    public static class LoanMapper
    {
        public static LoanDto ToLoanDto(this Loan loan)
        {
            return new LoanDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
            };
        }

        public static Loan ToLoanFromCreateDto(this CreateLoanRequestDto loan)
        {
            return new Loan
            {
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                Borrower_Id = loan.Borrower_Id,
                Book_Id = loan.Book_Id,
            };
        }

        public static Loan ToLoanFromUpdateDto(this UpdateLoanRequestDto loan)
        {
            return new Loan
            {
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
            };
        }

        public static LoanBookIncludedDto ToLoanBookIncludedDto(this Loan loan)
        {
            return new LoanBookIncludedDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                Book = loan.Book.ToBookIncludeDto(),
            };
        }

        public static LoanBorrowerIncludedDto ToLoanBorrowerIncludedDto(this Loan loan)
        {
            return new LoanBorrowerIncludedDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                Borrower = loan.Borrower.ToBorrowerIncludeDto(),

            };
        }

        public static LoanIncludeBorrowerBookDto ToLoanIncludeBorrowerBookDto(this Loan loan)
        {
            return new LoanIncludeBorrowerBookDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                Book = loan.Book.ToBookIncludeDto(),
                Borrower = loan.Borrower.ToBorrowerIncludeDto(),
            };
        }
    }
}
