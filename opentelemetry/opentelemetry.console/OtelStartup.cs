using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using opentelemetry.biz;
using OpenTelemetry.Logs;

namespace opentelemetry.console
{
    internal class OtelStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IService, Service>(provider => 
                    new Service(provider.GetRequiredService<ILogger<Service>>(),
                        new Dependent(provider.GetRequiredService<ILogger<Dependent>>())))

                // Logs
                .AddLogging(builder =>
                {
                    builder
                        .ClearProviders()
                        .AddOpenTelemetry(options =>
                        {
                            options.AddConsoleExporter();
                            options.IncludeScopes = true;
                            options.IncludeFormattedMessage= true;
                        });
                });
        }
    }
}