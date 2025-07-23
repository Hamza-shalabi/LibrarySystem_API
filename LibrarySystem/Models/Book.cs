using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Book: IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public long ISBN { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Loan> Loans { get; set; }
        [ForeignKey("Author")]
        public int Author_Id { get; set; }
        public Author Author { get; set; }
    }
}
