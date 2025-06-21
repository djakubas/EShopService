using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Domain.Models;
using System.Security.Cryptography;
using System.Globalization;
using System.Security.Authentication;
using User.Domain.Exceptions;

namespace User.Application
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GenerateToken(string userId, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("../data/privatekey.pem"));

            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IJwtTokenService
    {
        string GenerateToken(string userId, List<string> roles);
    }
}
