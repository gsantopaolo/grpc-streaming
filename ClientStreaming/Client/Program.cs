
using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Sensorsystem;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new SensorService.SensorServiceClient(channel);

            Task unarySend = SendUnaryDataAasync(client);
            Task clientStreaming = SendStremingDataAasync(client);

            await Task.WhenAll(unarySend, clientStreaming);


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task SendUnaryDataAasync(SensorService.SensorServiceClient client)
        {
            await Task.Delay(2000);
            var reply = await client.GetAvailableSensorsAsync(new Sensorsystem.AvailableSensorsRequest { Username = "username", Message = "message from client", });
            Console.WriteLine($"Server response: { reply.Message }");
            Console.WriteLine($"Devices from server: { reply.Devices }");
        }

        private static async Task SendStremingDataAasync(SensorService.SensorServiceClient client)
        {
            using var call = client.SendSensorData();
            for (var i = 0; i < 20; i++)
            {
                await call.RequestStream.WriteAsync(new SensorData { Data1 = $"Message{i}, Data2 = {i}" });
                await Task.Delay(200);
            }

            await call.RequestStream.CompleteAsync();

            var response = await call;
            Console.WriteLine($"Message response: {response.Message}");
        }
    }
}
