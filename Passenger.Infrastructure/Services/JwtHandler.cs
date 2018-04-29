using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Passenger.Infrastructure.DTO;
using Passenger.Infrastructure.Settings;
using Passenger.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Passenger.Infrastructure.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _settings;
        public JwtHandler(JwtSettings settings)
        {
            _settings = settings;
        }
        public JwtDto CreateToken(string email, string role)
        {
            var now = DateTime.UtcNow;
            // za pomocą claimsów możemu ustawiać dane ... (nie mogę zrozumieć)
            var claims = new Claim[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, email),  // dla kogo tworzymy token  
               new Claim(ClaimTypes.Role, role), // rola - admin lub zwykły uzytkownik
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ), //identyfikator
               new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString(), ClaimValueTypes.Integer64 ) // linuksowy epoch - format daty
            };
            var expires = now.AddMinutes(_settings.ExpiryMinutes);
            // to jak będziemy walidowac nasze tokeny
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                SecurityAlgorithms.HmacSha256);
            // tworzymy nasz token
            var jwt = new JwtSecurityToken(
                issuer: _settings.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials //klucz do tworzenia tokena i jego zabezpieczenia
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JwtDto
            {
                Token = token,
                Expires = expires.ToTimestamp()
            };
        }
    }
}