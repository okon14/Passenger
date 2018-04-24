using System;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Services
{
    public interface IJwtHandler
    {
         //Interfejs odpowiedzialny za obsługe naszych tokenów - generowniae po stronie API
         JwtDto CreateToken(string email, string role);
    }
}