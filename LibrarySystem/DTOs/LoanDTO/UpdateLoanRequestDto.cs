using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class UpdateLoanRequestDto
    {
        [Required]
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
