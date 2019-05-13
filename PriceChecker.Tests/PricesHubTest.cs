using Xunit;
using PriceChecker.Hubs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;

namespace PriceChecker.Tests
{
    public class PricesHubTest
    {
        [Theory]
        [InlineData("sdadfafa", "")]
        public async Task GetProductsTest(string input, string output)
        {
            var response = await StartTestServer(input, "getProducts");
            Assert.Equal(response, output);
        }

        [Theory]
        [InlineData("sdadfafa", "")]
        public async Task GetPricesTesk(string input, string output)
        {
            var response = await StartTestServer(input, "getPrices");
            Assert.Equal(response, output);
        }

        private async Task<string> StartTestServer(string input, string method)
        {
            var response = string.Empty;
            TestServer server = null;
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseSignalR(routes => routes.MapHub<PricesHub>("/priceChecker"));
                });
            server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/priceChecker",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();
            connection.On<string>(method, msg =>
            {
                response = msg;
            });
            await connection.StartAsync();
            await connection.InvokeAsync(method, input);
            return response;
        }
    }
}
