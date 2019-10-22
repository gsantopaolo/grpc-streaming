using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {


public static int Main()
    {
        // Reset unsupported character encoding for exotic languages to en-US
        ConsoleHelper.FixEncoding();

        // Show the message where the user can see it
        if (ConsoleHelper.IsInteractiveAndVisible)
        {
            // Console is visible, also use colour for this text
            ConsoleHelper.WriteLine("Hello console.", ConsoleColor.Red);
        }
        else
        {
            // Console is not visible, choose another output (non-interactive session,
            // no console window allocated, or output redirected)
            //MessageBox.Show("Hello window.");
        }

        // Interaction only if it is possible
        if (!ConsoleHelper.IsInputRedirected)
        {
            Console.Write("Please enter your name: ");
            Console.ReadLine();
        }

        // Move cursor and clear line
        Console.Write("Your name is:");
        ConsoleHelper.MoveCursor(-3);
        Console.Write("needs more checking...");
        ConsoleHelper.ClearLine();   // Oh well, doesn’t matter anyway.

        // Progress bar only if the output is interactive and can be overwritten. Otherwise,
        // all intermediate frames end up in the file being redirected to, in some form.
        if (!ConsoleHelper.IsOutputRedirected)
        {
            // Activate progress bar and update it regularly
            ConsoleHelper.ProgressTitle = "Downloading";
            ConsoleHelper.ProgressTotal = 10;
            for (int i = 0; i <= 10; i++)
            {
                ConsoleHelper.ProgressValue = i;
                Thread.Sleep(500);
                // Warning and error state is displayed in colour (yellow/red instead of green)
                if (i >= 5)
                {
                    ConsoleHelper.ProgressHasWarning = true;
                }
                if (i >= 8)
                {
                    ConsoleHelper.ProgressHasError = true;
                }
            }
            // Remove progress bar again
            ConsoleHelper.ProgressTotal = 0;
        }

        // Show long text with proper word wrapping
        ConsoleHelper.WriteWrapped("This very long text must be wrapped at the right end of the console window. But that should not tear apart words just where the line is over but move the excess word to the next line entirely. This will regard the actual width of the window.");

        ConsoleHelper.WriteWrapped("This also works for tabular output like a listing of command line parameters:");

        // Line wrapping for tabular output
        ConsoleHelper.WriteWrapped("  /a    Just a short note.", true);
        ConsoleHelper.WriteWrapped("  /b    The text in the following lines is wrapped so that it continues under the last content column (that is this description). That is recognised by the last occurrence of two spaces.", true);
        ConsoleHelper.WriteWrapped("  /cde  Nothing important, really.", true);

        // Clear input buffer to not use any premature keystrokes
        ConsoleHelper.ClearKeyBuffer();
        // Confirmation message with timeout (15 seconds) and vanishing dots
        ConsoleHelper.Wait("Seen it all?", 15, true);

        // Simple wait for any input key (not Ctrl, Shift, NumLock, Mute, and so on)
        ConsoleHelper.Wait();

        // Prevent closing the console window after program end when running with the
        // debugger in Visual Studio. Without the debugger, Visual Studio will wait
        // already. Conforming things here.
        ConsoleHelper.WaitIfDebug();

        // Show a red alert message and terminate the process with an exit code (12)
        return ConsoleHelper.ExitError("All is lost!", 12);
    }
    //const string URL = "http://grpcservices.softwarelab.it:12345";
    const string URL = "http://localhost:12345";
        static CancellationTokenSource cts;
        static async Task Main2(string[] args)
        {
            ConsoleHelper2.DrawRectangle(118, 10, new System.Drawing.Point(0, 0), ConsoleColor.Blue);
            ConsoleHelper2.DrawLine(1, 2, 118, ConsoleColor.Blue);
            ConsoleHelper2.WriteCenteredText("gRPC Client",1);
            Console.ReadLine();
            Console.SetCursorPosition(0, 0);
            Console.Write("###############################################");
            Console.SetCursorPosition(0, 1);
            Console.Write("#                                             #");
            Console.SetCursorPosition(0, 2);
            Console.Write("#         gRPC Client                         #");
            Console.SetCursorPosition(0, 3);
            Console.Write("###############################################");
            for (int row = 4; row < 10; row++)
            {
                Console.SetCursorPosition(0, row);
                Console.Write("#                                             #");
            }
            Console.SetCursorPosition(0, 10);
            Console.Write("###############################################");

            int data = 1;
            System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
            clock.Start();
            while (true)
            {
                data++;
                Console.SetCursorPosition(4, 5);
                Console.Write($"Current Value: {data}");
                Console.SetCursorPosition(4, 6);
                Console.Write($"Running Time: { Math.Truncate(clock.Elapsed.TotalMinutes)} minutes { Math.Truncate(clock.Elapsed.TotalSeconds)} seconds");
                Thread.Sleep(1000);
            }

            Console.ReadKey();



            Console.WriteLine("starting client");
            Console.WriteLine($"Connecting to {URL}");

            // Instantiate the CancellationTokenSource.
            cts = new CancellationTokenSource();

            StreamHelper s1 = new StreamHelper(URL);
            StreamHelper s2 = new StreamHelper(URL);
            StreamHelper s3 = new StreamHelper(URL);
            StreamHelper s4 = new StreamHelper(URL);

            Task worker1 = s1.StartStreaming(cts.Token, "Worker 1");
            Task worker2 = s2.StartStreaming(cts.Token, "Worker 2");
            Task worker3 = s3.StartStreaming(cts.Token, "Worker 3");
            Task worker4 = s4.StartStreaming(cts.Token, "Worker 4");

            Console.WriteLine($"Hit ESC to stop sendig");
            while (true)
            {
                var result = Console.ReadKey(intercept: true);
                if (result.Key == ConsoleKey.Escape)
                {
                    break;
                }

                cts.Cancel();
                Console.WriteLine($"Stopping all streming");
            }

            Task.WaitAll(new[] { worker1, worker2, worker3, worker4 });
            Console.WriteLine($"Hit any key to quit");
            Console.ReadLine();
        }

        #region channe and client inizializations tests
        ////ChannelOption o = new ChannelOption(ChannelOptions.SslTargetNameOverride, )
        //// client
        //// This switch must be set before creating the GrpcChannel/HttpClient.
        //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        //    //var channel = new Channel("localhost", 5000, ChannelCredentials.Insecure);

        //    //var channel = new Channel("grpcservices.westeurope.cloudapp.azure.com", 5000, ChannelCredentials.Insecure);
        //    //var client = new VideoStream.VideoStream1.VideoStream1Client(channel);
        //    //var channel = GrpcChannel.ForAddress("http://localhost:5000");
        //    //var client = new VideoStream.VideoStream1.VideoStream1Client(channel);
        //    //var httpClientHandler = new HttpClientHandler();
        //    //// Return `true` to allow certificates that are untrusted/invalid

        //    //httpClientHandler.ServerCertificateCustomValidationCallback =
        //    //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        //    //var httpClient = new HttpClient(httpClientHandler);
        //    //var channel = GrpcChannel.ForAddress("https://grpcservices.westeurope.cloudapp.azure.com:5000", new GrpcChannelOptions { HttpClient = httpClient });



        //    //HttpClient httpClient = new HttpClient();
        //    //httpClient.Timeout = Timeout.InfiniteTimeSpan;

        //    //var channel = GrpcChannel.ForAddress("http://grpcservices.softwarelab.it:12345", new GrpcChannelOptions
        //    //{
        //    //    HttpClient = httpClient
        //    //});
        #endregion

    }

    class StreamHelper
    {
        public string gRPC_Streaming_Service { get; }

        public StreamHelper(string streamingService)
        {
            gRPC_Streaming_Service = streamingService;
        }
        public async Task StartStreaming(CancellationToken ct, string workerId = null)
        {
            // IMOPRTANT! only for dev purpose run service on http instead of https
            // This switch shall be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(gRPC_Streaming_Service);
            var client = new Stream.StreamerService.StreamerServiceClient(channel);
            

            using (var call = client.Stream())
            {
                //need to start listening at incoming messages first otherwise there is the risk of loosing some initial messages
                #region reading incoming stream
                //Read incoming messages in a background task
                var readTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        try
                        {
                            var stream = call.ResponseStream.Current;
                            UpdateFPS(workerId);

                            if (ct.IsCancellationRequested)
                                break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });
                #endregion

                #region writing outgoing stream
                var sendTask = Task.Run(async () =>
                {
                    // Write outgoing messages 
                    bool doWork = true;
                    while (doWork)
                    {
                        try
                        {
                            var frame = new Stream.Frame() { Username = "username", Message = "hello" };
                            frame.Content = Google.Protobuf.ByteString.CopyFrom(new byte[100000]);
                            await call.RequestStream.WriteAsync(frame);
                            if (ct.IsCancellationRequested)
                                doWork = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            doWork = false;
                        }
                    }
                });
                #endregion

               
                await readTask;
                await sendTask;
                Task.WaitAll(new[] { readTask, sendTask});

                // Finish call and report results
                await call.RequestStream.CompleteAsync();
            }
        }

        static DateTime _lastTime; // marks the beginning the measurement began
        static int _framesRendered; // an increasing count
        static int _fps; // the FPS calculated from the last measurement
        static void UpdateFPS(string workerId)
        {
            _framesRendered++;
            if (_lastTime == null)
                _lastTime = DateTime.Now;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                // one second has elapsed 

                _fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }

            // draw FPS on screen here using current value of _fps       
            Console.WriteLine($"Worker {workerId} - {_fps} FPS");
        }
    }

}

