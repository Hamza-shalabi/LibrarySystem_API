using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.BorrowerDTO;

namespace LibrarySystem.DTOs.LoanDTO
{
    public class LoanDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
