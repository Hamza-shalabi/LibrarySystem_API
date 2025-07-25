﻿namespace LibrarySystem.Interface
{
    public interface IPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashPassword);
    }
}
