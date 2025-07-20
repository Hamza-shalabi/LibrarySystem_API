using LibrarySystem.Models;

namespace LibrarySystem.Interface
{
    public interface IAuthenticationRepository
    {
        public Task<User> RegisterAsync(User user);
        public Task<User?> LoginAsync(string email);
    }
}
