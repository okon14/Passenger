namespace Passenger.Infrastructure.Settings
{
    public class GeneralSettings
    {
        // właściwosć ustawiana automatycznie z wartosci appsettings.json z użyciem Autofac
        public string Name { get; set; }
        public bool SeedData { get; set; } //czy inicjalizujemy dane automatem czy nie?
    }
}