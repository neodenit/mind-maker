using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neodenit.MindMaker.Services.GPT3.Converters;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class Program
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("local.settings.json", true).AddEnvironmentVariables().Build();

            Settings settings = new();
            config.GetSection("Settings").Bind(settings);

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IBranchConverter, BranchConverter>();
                    services.AddSingleton(settings);
                })
                .Build();

            host.Run();
        }
    }
}