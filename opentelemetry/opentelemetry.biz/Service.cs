using System;
using Microsoft.Extensions.Logging;

namespace opentelemetry.biz
{
    public class Service : IService
    {
        private readonly ILogger<Service> _logger;
        private readonly IService _dependent;

        public Service(ILogger<Service> logger, IService dependent)
        {
            _logger = logger;
            _dependent = dependent;
        }

        public void Serv()
        {
            _dependent.Serv();
            _logger.LogInformation("Hello from {name} {price}.", "tomato", 2.99);
        }
    }
}
