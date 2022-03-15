using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using opentelemetry.biz;

namespace opentelemetry.console
{
    internal class PlainStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IService, Service>(provider =>
                    new Service(provider.GetRequiredService<ILogger<Service>>(),
                        new Dependent(provider.GetRequiredService<ILogger<Dependent>>())))
                .AddLogging();
        }
    }
}