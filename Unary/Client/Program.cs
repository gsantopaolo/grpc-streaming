
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

            var reply = await client.GetAvailableSensorsAsync(new Sensorsystem.AvailableSensorsRequest { Username = "username", Message = "message from client", });
            Console.WriteLine($"Server response: { reply.Message }");
            Console.WriteLine($"Devices from server: { reply.Devices }");
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
