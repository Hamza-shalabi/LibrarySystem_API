namespace LibrarySystem.DTOs.BookDTO
{
    public class BookIncludeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public long ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
