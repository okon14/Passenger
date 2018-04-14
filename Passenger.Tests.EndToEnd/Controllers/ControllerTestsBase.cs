using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Passenger.Api;

namespace Passenger.Tests.EndToEnd.Controllers
{
    public class ControllerTestsBase
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client; // Będzie wykonywać zapytania do naszego servera
        protected ControllerTestsBase()
        {
            // Tworzy w pełni działające API w pmięci
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        // Pomocnicza metoda do stworzenia z obiektu jego wersji w jsonie
        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            // Content-Type: "application/json"
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}