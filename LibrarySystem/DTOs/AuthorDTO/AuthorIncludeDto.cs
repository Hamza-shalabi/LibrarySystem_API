namespace LibrarySystem.DTOs.AuthorDTO
{
    public class AuthorIncludeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}
