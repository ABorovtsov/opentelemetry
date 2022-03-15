using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using opentelemetry.biz;

namespace opentelemetry.console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var startup = new PlainStartup();
            //var startup = new OtelStartup();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(startup.ConfigureServices)
                .Build();
            var service = host.Services.GetRequiredService<IService>();
            
            service.Serv();
        }
    }
}
