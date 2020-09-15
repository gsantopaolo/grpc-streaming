using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Sensorsystem;

namespace Server
{
    public class SensorHub : SensorService.SensorServiceBase
    {
        private readonly ILogger<SensorHub> _logger;
        public SensorHub(ILogger<SensorHub> logger)
        {
            _logger = logger;
        }



        public override Task<AvailableSensorsResponse> GetAvailableSensors(AvailableSensorsRequest request, ServerCallContext context)
        {
            return Task.FromResult(new AvailableSensorsResponse
            {
                Message = "Hello from server",
                Devices = "This is the server replying with a list of devices"
            });
        }
    }
}
