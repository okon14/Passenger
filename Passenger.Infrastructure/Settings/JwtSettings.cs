namespace Passenger.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; } // klucz podpisujący tokeny
        public int ExpiryMinutes { get; set; } // ile czasu będze żył token
    }
}