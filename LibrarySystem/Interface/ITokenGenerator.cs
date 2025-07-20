using LibrarySystem.Models;

namespace LibrarySystem.Interface
{
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();

    }
}
