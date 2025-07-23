using System.ComponentModel.DataAnnotations;
using LibrarySystem.Interface;

namespace LibrarySystem.Models
{
    public class Author: IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Bio { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime LastUpdatedAt { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
