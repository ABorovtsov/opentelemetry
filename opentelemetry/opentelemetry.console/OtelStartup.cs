using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using opentelemetry.biz;
using OpenTelemetry.Logs;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace opentelemetry.console
{
    internal class OtelStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "opentelemetry";
            var activityListener = new ActivityListener
            {
                ShouldListenTo = s => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> activityOptions) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) => ActivitySamplingResult.AllData
            };
            var activitySource = new ActivitySource(appName);
            ActivitySource.AddActivityListener(activityListener);

            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.With(new DiagnosticActivityEnricher())
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();


            services.AddTransient<IService, Service>(provider =>
                    new Service(provider.GetRequiredService<ILogger<Service>>(),
                        new Dependent(provider.GetRequiredService<ILogger<Dependent>>(), activitySource),
                        activitySource))
                // Logs
                .AddLogging(builder =>
                {
                    builder
                        .ClearProviders()
                        //.AddOpenTelemetry(options =>
                        //{
                        //    options.AddConsoleExporter();
                        //    options.IncludeScopes = true;
                        //    options.IncludeFormattedMessage = true;
                        //})
                        .AddSerilog(serilogLogger)
                        .AddSimpleConsole(options =>
                        {
                            options.IncludeScopes = true;
                        })
                        // Writes to a cross-platform event source with the name Microsoft-Extensions-Logging. On Windows, the provider uses ETW
                        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0#event-source
                        // How to collect:
                        // > dotnet trace ps | findstr opentelemetry.console
                        // > dotnet trace collect -p 93812 --providers Microsoft-Extensions-Logging
                        .AddEventSourceLogger()
                        .Configure(options => options.ActivityTrackingOptions = 
                            ActivityTrackingOptions.SpanId
                            | ActivityTrackingOptions.TraceId
                            | ActivityTrackingOptions.ParentId
                            | ActivityTrackingOptions.Baggage
                            | ActivityTrackingOptions.Tags);
                });
        }
    }
}