using System.Collections.Generic;
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
            var methodName = GetFullMethodName(MethodBase.GetCurrentMethod());
            Activity.Current?.AddEvent(new ActivityEvent(methodName, default, new ActivityTagsCollection(new List<KeyValuePair<string, object?>>
            {
                new("customTag", 25)
            })));
            
            using var activity = _activitySource.StartActivity(name:$"{methodName}", kind: ActivityKind.Server);
            activity?.SetTag("name", "apple");
            activity?.AddEvent(new ActivityEvent(methodName));
            activity?.SetStatus(ActivityStatusCode.Ok, "BlaBla");
            activity?.SetBaggage("BaggageKey", "BaggageValue from Dependent");


            _logger.LogInformation("Hello from {name} {price}.", "apple", 1.42);
        }

        public string GetFullMethodName(MethodBase m)
        {
            return $"{m?.DeclaringType?.Name}.{m?.Name}";
        }
    }
}
