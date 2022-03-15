using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace opentelemetry.biz
{
    public class Dependent : IService
    {
        private readonly ILogger<Dependent> _logger;
        private readonly ActivitySource _activitySource;

        public Dependent(ILogger<Dependent> logger, ActivitySource activitySource)
        {
            _logger = logger;
            _activitySource = activitySource;
        }

        public void Serv()
        {
            var activityName = $"{nameof(Dependent)} -> {MethodBase.GetCurrentMethod()?.Name}";

            using var activity = _activitySource.StartActivity($"{activityName}", ActivityKind.Server);
            activity?.SetTag("name", "apple");
            activity?.Parent?.AddEvent(new ActivityEvent(activityName));

            _logger.LogInformation("Hello from {name} {price}.", "apple", 1.42);
        }
    }
}
