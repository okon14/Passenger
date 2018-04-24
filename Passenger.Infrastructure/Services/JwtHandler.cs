using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Passenger.Infrastructure.DTO;
using Passenger.Infrastructure.Settings;
using Passenger.Infrastructure.Extensions;

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

            //var signi 41.01
            return new JwtDto();
        }
    }
}