using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Sensorsystem;

namespace Server
{
    public class SensorHub : SensorService.SensorServiceBase
    {
        private readonly ILogger<SensorHub> _logger;
        private static Random randomizer = new Random();
        public SensorHub(ILogger<SensorHub> logger)
        {
            _logger = logger;
        }

        public override Task<AvailableSensorsResponse> GetAvailableSensors(AvailableSensorsRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"received a GetAvailableSensors request : {request.Message}");

            return Task.FromResult(new AvailableSensorsResponse
            {
                Message = "Hello from server",
                Devices = "This is the server replying with a list of devices"
            });
        }

        public override async Task<SensorDataResponse> SendSensorData(IAsyncStreamReader<SensorData> requestStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"received some data from client Data1: {message.Data1}, Data2: {message.Data2}");
            }

            return new SensorDataResponse { Message = "Streamed data received correctly" };
        }

        public override async Task<Empty> SendSensorDataNoResponse(IAsyncStreamReader<SensorData> requestStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"received some data from client Data1: {message.Data1}, Data2: {message.Data2}");
            }

            return new Empty();
        }

        public override async Task ReceiveTemperatureUpdates(TemperatureRequest request, IServerStreamWriter<TemperatureData> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"received request to start stream temperature data from device id: {request.Deviceid}");

            while (!context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Sending temperature data to device id: {request.Deviceid}.");

                await responseStream.WriteAsync(new TemperatureData { Devicelocation = "Switzerland", Temperature = randomizer.Next(1, 100)});

                await Task.Delay(500);
            }

            _logger.LogInformation($"stop streaming temperature data to device id: {request.Deviceid}");
        }
    }
}
