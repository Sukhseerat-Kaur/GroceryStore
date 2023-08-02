using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using GroceryStoreCore.DTOs;
using GroceryStoreCore.Utilities.Configuration.Interfaces;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAppConfiguration _appConfiguration;
        public AuthService(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public string Authenticate(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.JwtSecretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole),
            };

            var token = new JwtSecurityToken(
                _appConfiguration.JwtIssuer,
                _appConfiguration.JwtAudience,
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}