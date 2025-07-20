using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.BookDTO;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class LoanBorrowerToBookDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BookIncludeDto Book { get; set; }
    }
}
