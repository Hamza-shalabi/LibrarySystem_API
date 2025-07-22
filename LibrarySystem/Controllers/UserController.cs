using System.Security.Claims;
using LibrarySystem.DTOs.UserDTO;
using LibrarySystem.Interface;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using LibrarySystem.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(
        IGenericRepository<User> repo,
        IPasswordService passwordService,
        ITokenGenerator tokenService,
        ITokenValidator tokenValidator,
        ITokenRepository tokenRepo,
        UserService service
        ) : ControllerBase
    {
        private readonly IGenericRepository<User> _repo = repo;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenGenerator _tokenService = tokenService;
        private readonly ITokenValidator _tokenValidator = tokenValidator;
        private readonly ITokenRepository _tokenRepo = tokenRepo;
        private readonly UserService _service = service;

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _service.GetUser(id);
            if( user == null)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });
            return Ok(user);
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

            var user = await _service.UpdateUser(id, userDto);
            if (user == null)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });

            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isExist = await _service.DeleteUser(id);
            if (!isExist)
                return NotFound(new Error { ErrorMessage = $"There is No User With This ID {id}" });
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

            var user = await _service.RegisterUser(userDto);
            if (user == null)
                return Conflict(new Error { ErrorMessage = "Email Already Exists or Passwords Do Not Match" });

            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var token = await _service.LoginUser(userDto);
            if (token == null)
                return Unauthorized(new Error { ErrorMessage = "Invalid Email or Password" });

            return Ok(token);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Error { ErrorMessage = $"Missing Requirements: {ModelState}" });

            var token = await _service.RefreshToken(refreshDto);
            if (token == null)
                return Unauthorized(new Error { ErrorMessage = "Invalid Refresh Token" });

            return Ok(token);

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
                await _service.Logout(id);
            }
            catch (Exception e)
            {
                return Unauthorized(new Error { ErrorMessage = $"No User Found {e}" });
            }

            return NoContent();
        }
    }
}