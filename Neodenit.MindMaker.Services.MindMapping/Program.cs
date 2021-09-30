using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Neodenit.MindMaker.Services.MindMapping
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => services.AddTransient<IHelpers, Helpers>())
                .Build();

            host.Run();
        }
    }
}