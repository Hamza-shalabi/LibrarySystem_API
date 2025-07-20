namespace LibrarySystem.Interface
{
    public interface ITokenValidator
    {
        public bool Validate(string refreshToken);
    }
}
