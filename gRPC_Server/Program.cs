using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Linq;
using VideoStream;

namespace gRPC_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "server")
            {
                // host server
                var server = new global::Grpc.Core.Server(new ChannelOption[]
                {
                    // new ChannelOption(ChannelOptions.MaxConcurrentStreams, 99999)
                })
                {
                    Services = { VideoStream1.BindService(new VideoStream1Implementation()) },
                    Ports = { new ServerPort("localhost", 12345, ServerCredentials.Insecure) }
                    
                };

                server.Start();

                Console.WriteLine($"gRPC Server started on port(s): {server.Ports.Select(x => x.Port)}");      
            }

            //if (args.Length == 0 || args[0] == "client")
            //{
            //    // client
            //    var channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);

            //    var client = new VideoStream.VideoStream1Client(channel);

            //    var tasks = Enumerable.Range(1, 10000).Select(async x =>
            //    {
            //        Console.WriteLine("client call: " + x);
            //        var streaming = client.sayHello(new HelloRequest());
            //        await streaming.ResponseStream.MoveNext();
            //    });

            //    //Task.WaitAll(tasks.ToArray());
            //}
            Console.ReadLine();
        }
    }
}
