using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class UpdateLoanRequestDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
