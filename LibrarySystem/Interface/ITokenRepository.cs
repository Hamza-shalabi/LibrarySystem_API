using LibrarySystem.Models;

namespace LibrarySystem.Interface
{
    public interface ITokenRepository
    {
        public Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken);
        public Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
        public Task<RefreshToken?> DeleteRefreshTokenAsync(int id);
        public Task DeleteAllRefreshTokensAsync(int id);
    }
}
