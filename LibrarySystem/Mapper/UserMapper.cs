using LibrarySystem.DTOs.UserDTO;
using LibrarySystem.Models;

namespace LibrarySystem.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
        }

        public static User ToUserFromLogin(this LoginUserRequestDto userDto)
        {
            return new User
            {
                Email = userDto.Email,
                Password = userDto.Password,
            };
        }

        public static User ToUserFromRegister(this RegisterUserRequestDto userDto)
        {
            return new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Password = userDto.Password,
                Role = userDto.Role ?? "User"
            };
        }

        public static User ToUserFromUpdate(this UpdateUserRequestDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Phone = userDto.Phone,
                Role = userDto.Role ?? "User"
            };
        }
    }
}
