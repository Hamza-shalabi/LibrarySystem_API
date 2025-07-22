using LibrarySystem.DTOs.UserDTO;
using LibrarySystem.Interface;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.ServiceLayer
{
    public class UserService(IGenericRepository<User> repo, IPasswordService passwordService, ITokenGenerator tokenService, ITokenValidator tokenValidator, ITokenRepository tokenRepo, IEmailSender emailSender)
    {
        private readonly IGenericRepository<User> _repo = repo;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenGenerator _tokenService = tokenService;
        private readonly ITokenValidator _tokenValidator = tokenValidator;
        private readonly ITokenRepository _tokenRepo = tokenRepo;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<UserDto?> GetUser(int id)
        {
            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return null;

            var user = await _repo.GetAsync(id);
            return user.ToUserDto();
        }

        public async Task<UserDto?> UpdateUser(int id, UpdateUserRequestDto userDto)
        {
            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return null;

            var originalUser = await _repo.GetAsync(id);
            originalUser.UserName = userDto.UserName;
            originalUser.Phone = userDto.Phone;

            var user = await _repo.UpdateAsync(originalUser);
            return user.ToUserDto();
        }

        public async Task<bool> DeleteUser(int id)
        {
            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<UserDto?> RegisterUser([FromBody] RegisterUserRequestDto userDto)
        {
            var email = await _repo.GetByValueAsync(x => x.Email == userDto.Email);
            if (email.Any())
                return null;

            if (userDto.Password != userDto.ConfirmPassword)
                return null;

            userDto.Password = _passwordService.HashPassword(userDto.Password);

            var user = await _repo.CreateAsync(userDto.ToUserFromRegister());
            await _emailSender.SendEmailAsync(userDto.Email, "Welcome to Library System", "Thank you for registering with us!");

            return user.ToUserDto();
        }

        public async Task<TokenDto?> LoginUser(LoginUserRequestDto userDto)
        {
            var listUser = await _repo.GetByValueAsync(u => u.Email == userDto.Email);
            var user = listUser.FirstOrDefault();
            if (user == null)
                return null;

            var isEqual = _passwordService.VerifyPassword(userDto.Password, user.Password);
            if (!isEqual)
                return null;

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenS = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
            };
            await _tokenRepo.CreateRefreshTokenAsync(refreshTokenS);
            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return tokenDto;
        }

        public async Task<TokenDto?> RefreshToken(RefreshTokenDto refreshDto)
        {
            var isValid = _tokenValidator.Validate(refreshDto.RefreshToken);
            if (!isValid)
                return null;

            var refreshToken = await _tokenRepo.GetByRefreshTokenAsync(refreshDto.RefreshToken);
            if (refreshToken == null)
                return null;

            var user = await _repo.GetAsync(refreshToken.UserId);
            if (user == null)
                return null;

            await _tokenRepo.DeleteRefreshTokenAsync(refreshToken.Id);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenS = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
            };
            await _tokenRepo.CreateRefreshTokenAsync(refreshTokenS);
            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
            };

            return tokenDto;

        }

        public async Task<bool> Logout(int id)
        {
            await _tokenRepo.DeleteAllRefreshTokensAsync(id);
            return true;
        }
    }
}
