using System;
using Microsoft.Extensions.Logging;

namespace opentelemetry.biz
{
    public class Dependent : IService
    {
        private readonly ILogger<Dependent> _logger;

        public Dependent(ILogger<Dependent> logger)
        {
            _logger = logger;
        }

        public void Serv()
        {
            _logger.LogInformation("Hello from {name} {price}.", "apple", 1.42);
        }
    }
}
