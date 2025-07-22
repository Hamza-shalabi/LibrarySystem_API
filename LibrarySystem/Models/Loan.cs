using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Loan: IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        [ForeignKey("Book")]
        public int Book_Id { get; set; }
        [ForeignKey("Borrower")]
        public int Borrower_Id { get; set; }
        public Book Book { get; set; }
        public Borrower Borrower { get; set; }
    }
}
