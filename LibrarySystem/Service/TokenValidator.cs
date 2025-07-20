using System.Text;
using LibrarySystem.Interface;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Service
{
    public class TokenValidator(IConfiguration config) : ITokenValidator
    {
        private readonly IConfiguration _config = config;
        public bool Validate(string refreshToken)
        {
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefreshKey"]))
            };

            JsonWebTokenHandler handler = new JsonWebTokenHandler();

            try 
            {
                handler.ValidateTokenAsync(refreshToken, parameters);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
