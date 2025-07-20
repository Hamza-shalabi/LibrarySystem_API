using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class CreateLoanRequestDto
    {
        [Required]
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required]
        public int Book_Id { get; set; }
        [Required]
        public int Borrower_Id { get; set; }
    }
}
