using ISTA.SecureApp.Models;
using ISTA.SecureApp.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ISTA.SecureApp.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private string secretKey;

        public AuthService(IConfiguration configuration,UserRepository userRepository)
        {
            _userRepository = userRepository;
            secretKey = configuration["tokenkey"]!;
        }

        public string? Login(Login login)
        {
            login.Password = Encript.EncriptPassword(login.Password);
            var user = _userRepository.Login(login) ?? throw new ValidationException("User not found");
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            int now = DateTime.Now.Hour;
            DateTime expires = DateTime.Now.AddHours(24 - now);

            var tokeOptions = new JwtSecurityToken(
                    issuer: "ISTA",
                    audience: "ISTA",
                    claims: claims,
                    expires: expires,
                    signingCredentials: signinCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
