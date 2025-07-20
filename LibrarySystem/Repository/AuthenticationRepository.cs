using LibrarySystem.Data;
using LibrarySystem.Interface;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repository
{
    public class AuthenticationRepository(ApplicationDBContext context) : IAuthenticationRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<User?> LoginAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            return user;
        }

        public async Task<User> RegisterAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
