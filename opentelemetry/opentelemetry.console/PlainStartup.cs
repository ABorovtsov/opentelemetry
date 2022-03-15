using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using opentelemetry.biz;

namespace opentelemetry.console
{
    internal class PlainStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "opentelemetry";
            var activitySource = new ActivitySource(appName);
            
            services.AddTransient<IService, Service>(provider =>
                    new Service(provider.GetRequiredService<ILogger<Service>>(),
                        new Dependent(provider.GetRequiredService<ILogger<Dependent>>(), activitySource), activitySource))
                .AddLogging();
        }
    }
}