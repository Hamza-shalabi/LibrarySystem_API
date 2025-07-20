using System.ComponentModel.DataAnnotations;
using LibrarySystem.DTOs.LoanDTO;
using LibrarySystem.Models;

namespace LibrarySystem.DTOs.BorrowerDTO
{
    public class BorrowerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Phone { get; set; }
    }
}
