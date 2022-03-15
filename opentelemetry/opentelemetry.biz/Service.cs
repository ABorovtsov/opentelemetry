using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            using var activity = _activitySource.StartActivity($"{nameof(Service)} -> {MethodBase.GetCurrentMethod()?.Name}", ActivityKind.Server);
            activity?.SetTag("name", "tomato");
            _dependent.Serv();

            _logger.LogInformation("Hello from {name} {price}. Events: {events}", "tomato", 2.99, 
                string.Join(',', activity?.Events.Select(e=>e.Name) ?? new List<string>()));
        }
    }
}
