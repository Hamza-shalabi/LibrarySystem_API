using LibrarySystem.Interface;
using Sodium;

namespace LibrarySystem.Service
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            var hashBassword = PasswordHash.ArgonHashString(password);
            return hashBassword;
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            var isEqual = PasswordHash.ArgonHashStringVerify(hashPassword, password);
            return isEqual;
        }
    }
}
