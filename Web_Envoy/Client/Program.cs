
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
            var randomizer = new Random(); 
            var channel = GrpcChannel.ForAddress("http://localhost:5001");
            var client = new SensorService.SensorServiceClient(channel);

            Console.WriteLine("Please type the device ID:");
            var line = Console.ReadLine();

            int sensorId = 0;
            int.TryParse(line, out sensorId);


            using (var cli = client.StreamData())
            {
                _ = Task.Run(async () =>
                {
                    while (await cli.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                    {
                        var response = cli.ResponseStream.Current;
                        Console.WriteLine($"Receiving data from Sensor {response.SensorID} sent data1 {response.Data1}, data2 {response.Data2}");
                    }
                });


                Console.WriteLine("Press any key to send random data or Q to stop.");

                while ((line = Console.ReadLine()) != null)
                {
                    if (line.ToLower() == "q")
                    {
                        break;
                    }
                        
                    int data1 = randomizer.Next(1, 100), data2 = randomizer.Next(1, 100);
                    await cli.RequestStream.WriteAsync(new SensorData { SensorID = sensorId, Data1 = data1.ToString(), Data2 = data2});
                    Console.WriteLine($"Sending data Sensor {sensorId} data1 {data1}, data2 {data2}");
                }
            }

            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


    }
}
