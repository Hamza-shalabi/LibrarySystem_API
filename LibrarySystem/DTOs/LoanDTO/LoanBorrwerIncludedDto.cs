using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.BookDTO;
using LibrarySystem.DTOs.BorrowerDTO;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class LoanBorrowerIncludedDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowerIncludeDto Borrower { get; set; }

    }
}
