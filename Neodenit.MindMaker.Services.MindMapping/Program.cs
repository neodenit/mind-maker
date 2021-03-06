using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class Program
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("local.settings.json", true).AddEnvironmentVariables().Build();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IHelpers, Helpers>();
                    services.AddSingleton(_ => new CosmosClient(config["ConnectionString"]));
                })
                .Build();

            host.Run();
        }
    }
}