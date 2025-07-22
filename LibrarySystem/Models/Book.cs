using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Book: IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public long ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        [ForeignKey("Author")]
        public int Author_Id { get; set; }
        public Author Author { get; set; }
    }
}
