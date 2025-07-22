using System.Security.Claims;
using System.Text;
using LibrarySystem.Interface;
using LibrarySystem.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Service
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _accessKey;
        private readonly SymmetricSecurityKey _refreshKey;
        public TokenGenerator(IConfiguration config)
        {
            _config = config;
            _accessKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _refreshKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefreshKey"]));
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
               new Claim("Id", user.Id.ToString()),
               new Claim(ClaimTypes.Role, user.Role),
            };

            var creds = new SigningCredentials(_accessKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var creds = new SigningCredentials(_refreshKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = null,
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
