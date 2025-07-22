using System.ComponentModel.DataAnnotations;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Borrower: IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public long Phone { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Loan> Loan { get; set; } = new List<Loan>();
    }
}
