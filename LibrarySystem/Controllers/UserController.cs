using System.Security.Claims;
using LibrarySystem.DTOs.UserDTO;
using LibrarySystem.Interface;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(
        IGenericRepository<User> repo,
        IPasswordService passwordService,
        IAuthenticationRepository authRepo,
        ITokenGenerator tokenService,
        ITokenValidator tokenValidator,
        ITokenRepository tokenRepo
        ) : ControllerBase
    {
        private readonly IGenericRepository<User> _repo = repo;
        private readonly IAuthenticationRepository _authRepo = authRepo;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenGenerator _tokenService = tokenService;
        private readonly ITokenValidator _tokenValidator = tokenValidator;
        private readonly ITokenRepository _tokenRepo = tokenRepo;

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });

            var user = await _repo.GetAsync(id);
            return Ok(user.ToUserDto());
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UpdateUserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            if (userDto.Id != id)
                return BadRequest(new Error { ErrorMessage = "ID's Mismatch" });

            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });

            var originalUser = await _repo.GetAsync(id);
            originalUser.UserName = userDto.UserName;
            originalUser.Phone = userDto.Phone;

            var user = await _repo.UpdateAsync(originalUser);
            return Ok(user.ToUserDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isExist = await _repo.IsExistAsync(id);
            if (!isExist)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });

            var user = await _repo.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var email = await _repo.GetByValueAsync(x => x.Email == userDto.Email);
            if (email.Any())
                return Conflict(new Error { ErrorMessage = "a User With This Email Or User Name Already Exists!" });

            var name = await _repo.GetByValueAsync(x => x.UserName == userDto.UserName);
            if (name.Any())
                return Conflict(new Error { ErrorMessage = "a User With This Email Or User Name Already Exists!" });

            if (userDto.Password != userDto.ConfirmPassword)
                return BadRequest(new Error { ErrorMessage = "Password And Confirm Password Do Not Match!" });

            userDto.Password = _passwordService.HashPassword(userDto.Password);

            var user = await _authRepo.RegisterAsync(userDto.ToUserFromRegister());
            return Ok(user.ToUserDto());
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var user = await _authRepo.LoginAsync(userDto.Email);
            if (user == null)
                return Unauthorized(new Error { ErrorMessage = "Password or Email Are Incorrect" });

            var isEqual = _passwordService.VerifyPassword(userDto.Password, user.Password);
            if (!isEqual)
                return Unauthorized(new Error { ErrorMessage = "Password or Email Are Incorrect" });

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

            return Ok(tokenDto);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var isValid = _tokenValidator.Validate(refreshDto.RefreshToken);
            if (!isValid)
                return Unauthorized(new Error { ErrorMessage = "Invalid Refresh Token" });

            var refreshToken = await _tokenRepo.GetByRefreshTokenAsync(refreshDto.RefreshToken);
            if (refreshToken == null)
                return BadRequest(new Error { ErrorMessage = "Invalid Refresh Token" });

            var user = await _repo.GetAsync(refreshToken.UserId);
            if (user == null)
                return BadRequest(new Error { ErrorMessage = "Invalid Refresh Token" });

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

            return Ok(tokenDto);

        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                int id = int.Parse(HttpContext.User.FindFirstValue("Id"));
                await _tokenRepo.DeleteAllRefreshTokensAsync(id);
            }
            catch (Exception e)
            {
                return Unauthorized(new Error { ErrorMessage = $"No User Found {e}" });
            }

            return NoContent();
        }
    }
}
