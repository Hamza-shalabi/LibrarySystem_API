﻿namespace LibrarySystem.DTOs.UserDTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string Role { get; set; } = "User";
    }
}
