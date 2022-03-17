using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace opentelemetry.biz
{
    public class Service : IService
    {
        private readonly ILogger<Service> _logger;
        private readonly IService _dependent;
        private readonly ActivitySource _activitySource;

        public Service(ILogger<Service> logger, IService dependent, ActivitySource activitySource)
        {
            _logger = logger;
            _dependent = dependent;
            _activitySource = activitySource;
        }

        public void Serv()
        {
            var methodName = GetFullMethodName(MethodBase.GetCurrentMethod());

            using var activity = _activitySource.StartActivity(methodName, ActivityKind.Server);
            activity?.SetTag("name", "tomato1");
            activity?.SetTag("name", "tomato2");
            activity?.AddTag("name", "tomato3");
            _dependent.Serv();

            _logger.LogInformation("Hello from {name} {price}. Events: {events}", "tomato", 2.99, 
                string.Join(',', activity?.Events.Select(e=>e.Name) ?? new List<string>()));
        }

        public string GetFullMethodName(MethodBase m)
        {
            return $"{m?.DeclaringType?.Name}.{m?.Name}";
        }
    }
}
