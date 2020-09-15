//using Grpc.Core;
//using Grpc.Net.Client;
//using System;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using static VideoStream.VideoStream1;

//namespace Client
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("starting client");
//            var readTask = Task.Run(() =>
//            {
//                var channel = InizializeChannel();

//                Streamer streamer1 = new Streamer();
//                streamer1.Stream(channel);

//                Streamer streamer2 = new Streamer();
//                streamer2.Stream(channel);
//            });
//            Console.ReadLine();
//        }

//        private static GrpcChannel InizializeChannel()
//        {
//            //ChannelOption o = new ChannelOption(ChannelOptions.SslTargetNameOverride, )
//            // client
//            // This switch must be set before creating the GrpcChannel/HttpClient.
//            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

//            //var channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);

//            //var channel = new Channel("grpcservices.westeurope.cloudapp.azure.com", 5000, ChannelCredentials.Insecure);
//            //var client = new VideoStream.VideoStream1.VideoStream1Client(channel);
//            //var channel = GrpcChannel.ForAddress("http://localhost:5000");
//            //var client = new VideoStream.VideoStream1.VideoStream1Client(channel);
//            //var httpClientHandler = new HttpClientHandler();
//            //// Return `true` to allow certificates that are untrusted/invalid

//            //httpClientHandler.ServerCertificateCustomValidationCallback =
//            //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
//            //var httpClient = new HttpClient(httpClientHandler);
//            //var channel = GrpcChannel.ForAddress("https://grpcservices.westeurope.cloudapp.azure.com:5000", new GrpcChannelOptions { HttpClient = httpClient });

//            //HttpClient httpClient = new HttpClient();
//            //httpClient.Timeout = Timeout.InfiniteTimeSpan;

//            //var channel = GrpcChannel.ForAddress("http://grpcservices.softwarelab.it:12345", new GrpcChannelOptions
//            //{
//            //    HttpClient = httpClient
//            //});

//            var channel = GrpcChannel.ForAddress("http://grpcservices.softwarelab.it:12345");
//            return channel;
//            //Task.WaitAll(tasks.ToArray());
//        }
//    }

//    class Streamer
//    {
//        public void Stream(GrpcChannel channel)
//        {
//            var client = new VideoStream.VideoStream1.VideoStream1Client(channel);
//            //need to start listening at incoming messages first otherwise there is the risk of loosing some initial messages

//            using (var call = client.StreamVideo())
//            {
//                //Read incoming messages in a background task
//                var readTask = Task.Run(async () =>
//                {
//                    while (await call.ResponseStream.MoveNext())
//                    {
//                        try
//                        {
//                            var stream = call.ResponseStream.Current;
//                            UpdateFPS();
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine(ex.Message);
//                        }
//                    }
//                });

//            }
//        }

//        private void StreamRequest(VideoStream1Client client)
//        {
//            // Write outgoing messages in a background task
//            var writeTask = Task.Run(async () =>
//            {
//                using (var call = client.StreamVideo())
//                {
//                    // Write outgoing messages 
//                    bool doWork = true;
//                    while (doWork)
//                    {
//                        try
//                        {
//                            var frame = new VideoStream.VideoFrame() { Username = "username", Message = "hello" };
//                            frame.Frame = Google.Protobuf.ByteString.CopyFrom(new byte[2000]);
//                            await call.RequestStream.WriteAsync(frame);
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine(ex.Message);
//                            doWork = false;
//                        }
//                    }
//                }
//            });
//        }

//        static DateTime _lastTime; // marks the beginning the measurement began
//        static int _framesRendered; // an increasing count
//        static int _fps; // the FPS calculated from the last measurement
//        static void UpdateFPS()
//        {
//            _framesRendered++;
//            if (_lastTime == null)
//                _lastTime = DateTime.Now;

//            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
//            {
//                // one second has elapsed 

//                _fps = _framesRendered;
//                _framesRendered = 0;
//                _lastTime = DateTime.Now;
//            }

//            // draw FPS on screen here using current value of _fps       
//            Console.WriteLine($"{_fps} FPS");
//        }
//    }
//}
