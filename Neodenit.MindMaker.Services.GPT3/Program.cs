using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Neodenit.MindMaker.Services.GPT3
{
    public class Program
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("local.settings.json").Build();

            Settings settings = new();
            config.GetSection("Settings").Bind(settings);

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => services.AddSingleton<ISettings>(settings))
                .Build();

            host.Run();
        }
    }
}