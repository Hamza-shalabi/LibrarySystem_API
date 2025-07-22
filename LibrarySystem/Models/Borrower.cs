using System.ComponentModel.DataAnnotations;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Borrower: IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Loan> Loan { get; set; } = new List<Loan>();
    }
}
