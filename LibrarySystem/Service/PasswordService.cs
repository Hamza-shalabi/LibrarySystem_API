using LibrarySystem.Interface;
using Sodium;

namespace LibrarySystem.Service
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            var hashPassword = PasswordHash.ArgonHashString(password);
            return hashPassword;
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            var isEqual = PasswordHash.ArgonHashStringVerify(hashPassword, password);
            return isEqual;
        }
    }
}
