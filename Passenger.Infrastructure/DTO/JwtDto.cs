namespace Passenger.Infrastructure.DTO
{
    public class JwtDto
    {
        // trzyma dane naszego tokena
        public string Token { get; set; } 
        public long Expiry { get; set; }
    }
}