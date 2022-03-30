using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace opentelemetry.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource(Constants.ActivitySourceName)
                .AddAspNetCoreInstrumentation()
                //.AddConsoleExporter()
                .AddJaegerExporter(o =>
                {
                    o.AgentHost = "127.0.0.1";
                    o.AgentPort = 6831;
                })
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Constants.ServiceName))
                .Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
