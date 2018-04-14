using Microsoft.Extensions.Configuration;

namespace Passenger.Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T : new()  // T ma być klasą z konstruktorem
        {
            //użycie prostej releksji
            var section = typeof(T).Name.Replace("Settings", string.Empty);
            var configurationValue = new T(); // stwórz nowy domyślny obiekt kalsy T
            configuration.GetSection(section).Bind(configurationValue); //przypisz do ustawień z json przypisz do obiektu klasy T
            
            return configurationValue;
        }
    }
}