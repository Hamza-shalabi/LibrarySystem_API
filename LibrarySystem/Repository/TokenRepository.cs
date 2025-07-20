using LibrarySystem.Data;
using LibrarySystem.Interface;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repository
{
    public class TokenRepository(ApplicationDBContext context): ITokenRepository
    {
        private readonly ApplicationDBContext _context = context;
        public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task DeleteAllRefreshTokensAsync(int id)
        {
            var refreshTokens = _context.RefreshTokens.Where(r => r.UserId == id);
            foreach(var token in refreshTokens)
            {
                _context.RefreshTokens.Remove(token);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> DeleteRefreshTokenAsync(int id)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Id == id);
            if (refreshToken == null)
                return null;

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            var refreshToken1 = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
            if (refreshToken1 == null)
                return null;

            return refreshToken1;
        }
    }
}
