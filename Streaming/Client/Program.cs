using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        #region some tests
    //    public static int Main()
    //{
    //    // Reset unsupported character encoding for exotic languages to en-US
    //    ConsoleHelper.FixEncoding();

    //    // Show the message where the user can see it
    //    if (ConsoleHelper.IsInteractiveAndVisible)
    //    {
    //        // Console is visible, also use colour for this text
    //        ConsoleHelper.WriteLine("Hello console.", ConsoleColor.Red);
    //    }
    //    else
    //    {
    //        // Console is not visible, choose another output (non-interactive session,
    //        // no console window allocated, or output redirected)
    //        //MessageBox.Show("Hello window.");
    //    }

    //    // Interaction only if it is possible
    //    if (!ConsoleHelper.IsInputRedirected)
    //    {
    //        Console.Write("Please enter your name: ");
    //        Console.ReadLine();
    //    }

    //    // Move cursor and clear line
    //    Console.Write("Your name is:");
    //    ConsoleHelper.MoveCursor(-3);
    //    Console.Write("needs more checking...");
    //    ConsoleHelper.ClearLine();   // Oh well, doesn’t matter anyway.

    //    // Progress bar only if the output is interactive and can be overwritten. Otherwise,
    //    // all intermediate frames end up in the file being redirected to, in some form.
    //    if (!ConsoleHelper.IsOutputRedirected)
    //    {
    //        // Activate progress bar and update it regularly
    //        ConsoleHelper.ProgressTitle = "Downloading";
    //        ConsoleHelper.ProgressTotal = 10;
    //        for (int i = 0; i <= 10; i++)
    //        {
    //            ConsoleHelper.ProgressValue = i;
    //            Thread.Sleep(500);
    //            // Warning and error state is displayed in colour (yellow/red instead of green)
    //            if (i >= 5)
    //            {
    //                ConsoleHelper.ProgressHasWarning = true;
    //            }
    //            if (i >= 8)
    //            {
    //                ConsoleHelper.ProgressHasError = true;
    //            }
    //        }
    //        // Remove progress bar again
    //        ConsoleHelper.ProgressTotal = 0;
    //    }

    //    // Show long text with proper word wrapping
    //    ConsoleHelper.WriteWrapped("This very long text must be wrapped at the right end of the console window. But that should not tear apart words just where the line is over but move the excess word to the next line entirely. This will regard the actual width of the window.");

    //    ConsoleHelper.WriteWrapped("This also works for tabular output like a listing of command line parameters:");

    //    // Line wrapping for tabular output
    //    ConsoleHelper.WriteWrapped("  /a    Just a short note.", true);
    //    ConsoleHelper.WriteWrapped("  /b    The text in the following lines is wrapped so that it continues under the last content column (that is this description). That is recognised by the last occurrence of two spaces.", true);
    //    ConsoleHelper.WriteWrapped("  /cde  Nothing important, really.", true);

    //    // Clear input buffer to not use any premature keystrokes
    //    ConsoleHelper.ClearKeyBuffer();
    //    // Confirmation message with timeout (15 seconds) and vanishing dots
    //    ConsoleHelper.Wait("Seen it all?", 15, true);

    //    // Simple wait for any input key (not Ctrl, Shift, NumLock, Mute, and so on)
    //    ConsoleHelper.Wait();

    //    // Prevent closing the console window after program end when running with the
    //    // debugger in Visual Studio. Without the debugger, Visual Studio will wait
    //    // already. Conforming things here.
    //    ConsoleHelper.WaitIfDebug();

    //    // Show a red alert message and terminate the process with an exit code (12)
    //    return ConsoleHelper.ExitError("All is lost!", 12);
    //}
        #endregion

        //const string URL = "http://grpcservices.softwarelab.it:12345";
        const string URL = "http://localhost:12345";
        static CancellationTokenSource cts;

        static PerformanceCounter cpuCounter;
        static PerformanceCounter cpuCounterThisApp;
        static PerformanceCounter ramCounter;

        static int maxThreads = 500;
        static int byteSize = 50000;

        static async Task Main(string[] args)
        {
            #region progressbar temporary removed 
            // Progress bar only if the output is interactive and can be overwritten. Otherwise,
            // all intermediate frames end up in the file being redirected to, in some form.
            //if (!ConsoleHelper.IsOutputRedirected)
            //{
            //    // Activate progress bar and update it regularly
            //    ConsoleHelper.ProgressTitle = "Downloading";
            //    ConsoleHelper.ProgressTotal = 10;
            //    for (int i = 0; i <= 10; i++)
            //    {
            //        ConsoleHelper.ProgressValue = i;
            //        Thread.Sleep(500);
            //        // Warning and error state is displayed in colour (yellow/red instead of green)
            //        if (i >= 5)
            //        {
            //            ConsoleHelper.ProgressHasWarning = true;
            //        }
            //        if (i >= 8)
            //        {
            //            ConsoleHelper.ProgressHasError = true;
            //        }
            //    }
            //    // Remove progress bar again
            //    ConsoleHelper.ProgressTotal = 0;
            //}
            #endregion

            // Set the Title 
            Console.Title = "gRPC Client";

            while (true)
            {
                Console.WriteLine("Total Threads:");
                string s = Console.ReadLine();
                maxThreads = int.Parse(s);

                Console.WriteLine("Message size in bytes:");
                s = Console.ReadLine();
                byteSize = int.Parse(s);

                Console.Clear();

                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);

                //This gets this app PID
                cpuCounterThisApp = new PerformanceCounter("Process", "% Processor Time", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, true);
                ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
                //System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                WriteTitle();

                ConsoleHelper.DrawRectangle(110, 20, new System.Drawing.Point(4, 8));
                ConsoleHelper.DrawLine(5, 10, 110);
                //ConsoleHelper.WriteCenteredText("gRPC Client", 0, ConsoleColor.White);

                // Instantiate the CancellationTokenSource.
                cts = new CancellationTokenSource();

                var elapsedTime = Task.Run(() =>
                {
                    System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
                    clock.Start();
                    while (cts.IsCancellationRequested == false)
                    {
                        Console.SetCursorPosition(7, 11);
                        Console.Write($"Running Time: { Math.Truncate(clock.Elapsed.TotalMinutes)} minutes { Math.Truncate(clock.Elapsed.TotalSeconds)} seconds");
                        Console.SetCursorPosition(7, 12);
                        // draw FPS on screen here using current value of _fps  
                        Console.Write($"Total Frames Rendered - {StreamHelper.TotalFramesRendered}");
                        Console.SetCursorPosition(7, 13);
                        Console.Write($"{StreamHelper.Fps} FPS");
                        Console.SetCursorPosition(7, 14);
                        Console.Write($"Total CPU % : { Math.Round(cpuCounter.NextValue(), 1)}");
                        Console.SetCursorPosition(7, 15);
                        Console.Write($"CPU Used by this process % : { Math.Round(cpuCounterThisApp.NextValue(), 1)}");
                        //Console.WriteLine($"CPU this app % : { Math.Round(cpuCounterThisApp.NextValue(), 1)}");
                        Console.SetCursorPosition(7, 16);
                        Console.Write($"Available Memory in MB : {  ramCounter.NextValue()}");
                        Console.SetCursorPosition(7, 17);
                        Console.Write($"Total threads: { maxThreads }");
                        Console.SetCursorPosition(7, 18);
                        Console.Write($"Message size in bytes : {  byteSize}");
                        Thread.Sleep(1000);
                    }
                });




                //Console.SetCursorPosition(8, 4);
                //Console.WriteLine("starting client");
                //Console.SetCursorPosition(9, 4);
                //Console.WriteLine($"Connecting to {URL}");



                List<Task> tasks = new List<Task>();
                //for (int i=0; i< maxThreads; i++)
                //{
                //    StreamHelper s1 = new StreamHelper(URL);
                //    Task worker1 = s1.StartStreaming(cts.Token);
                //    tasks.Add(worker1);
                //}

                tasks.Add(elapsedTime);


                // IMPORTANT Thread Vs Task
                //https://softwareengineering.stackexchange.com/questions/362475/what-construct-do-i-use-to-guarantee-100-tasks-are-running-in-parallel
                // Create the specified number of clients, to carry out test operations, each on their own threads
                Thread[] threads = new Thread[maxThreads];
                for (int count = 0; count < maxThreads; ++count)
                {
                    var index = count;
                    StreamHelper s1 = new StreamHelper(URL);
                    s1.ByteSize = byteSize;
                    threads[count] = new Thread(new ParameterizedThreadStart(s1.StartStreaming1))
                    {
                        Name = $"Client {count}" // for debugging
                    };
                    threads[count].Start(cts.Token);
                }


                Console.SetCursorPosition(7, 10);
                Console.Write($"Hit ESC to stop sendig");
                while (true)
                {
                    var result = Console.ReadKey(intercept: true);
                    if (result.Key == ConsoleKey.Escape)
                    {
                        cts.Cancel();
                        Console.SetCursorPosition(7, 19);
                        Console.WriteLine($"Stopping all stream");
                        break;
                    }
                }

                Task.WaitAll(tasks.ToArray());
                Console.Clear();
            }
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

        private static void WriteTitle()
        {
            var arr = new[]
                {
                    @"             ██████╗ ██████╗ ██████╗  ██████╗         ██████╗██╗     ██╗███████╗███╗   ██╗████████╗     ",
                    @"            ██╔════╝ ██╔══██╗██╔══██╗██╔════╝        ██╔════╝██║     ██║██╔════╝████╗  ██║╚══██╔══╝     ",
                    @"            ██║  ███╗██████╔╝██████╔╝██║             ██║     ██║     ██║█████╗  ██╔██╗ ██║   ██║        ",
                    @"            ██║   ██║██╔══██╗██╔═══╝ ██║             ██║     ██║     ██║██╔══╝  ██║╚██╗██║   ██║        ",
                    @"            ╚██████╔╝██║  ██║██║     ╚██████╗        ╚██████╗███████╗██║███████╗██║ ╚████║   ██║        ",
                    @"             ╚═════╝ ╚═╝  ╚═╝╚═╝      ╚═════╝         ╚═════╝╚══════╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝        ",
                };


            //Console.WindowWidth = 160;
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string line in arr)
                Console.WriteLine(line);
            Console.ResetColor();
        }
    }

    class StreamHelper
    {
        public string gRPC_Streaming_Service { get; }

        public StreamHelper(string streamingService)
        {
            gRPC_Streaming_Service = streamingService;
        }

        public int ByteSize { get; set; }


        public async void StartStreaming1(object ct)
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
                            UpdateFPS();

                            if (((CancellationToken)ct).IsCancellationRequested)
                            {
                                //Console.SetCursorPosition(7, consoleRow);
                                //Console.Write($"Stopping Worker {workerId}");
                                break;

                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.SetCursorPosition(7, consoleRow);
                            Console.Write(ex.Message);
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
                            frame.Content = Google.Protobuf.ByteString.CopyFrom(new byte[ByteSize]);
                            await call.RequestStream.WriteAsync(frame);
                            if (((CancellationToken)ct).IsCancellationRequested)
                                doWork = false;
                        }
                        catch (Exception ex)
                        {
                            //Console.SetCursorPosition(25, consoleRow);
                            //Console.Write(ex.Message);
                            doWork = false;
                        }
                    }
                });
                #endregion

                await readTask;
                await sendTask;
                Task.WaitAll(new[] { readTask, sendTask });

                // Finish call and report results
                await call.RequestStream.CompleteAsync();

            }
        }

        public async Task StartStreaming(CancellationToken ct)
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
                            UpdateFPS();

                            if (ct.IsCancellationRequested)
                            {
                                //Console.SetCursorPosition(7, consoleRow);
                                //Console.Write($"Stopping Worker {workerId}");
                                break;
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.SetCursorPosition(7, consoleRow);
                            Console.Write(ex.Message);
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
                            frame.Content = Google.Protobuf.ByteString.CopyFrom(new byte[200000]);
                            await call.RequestStream.WriteAsync(frame);
                            if (ct.IsCancellationRequested)
                                doWork = false;
                        }
                        catch (Exception ex)
                        {
                            //Console.SetCursorPosition(25, consoleRow);
                            //Console.Write(ex.Message);
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
        public static int Fps; // the FPS calculated from the last measurement
        public static int TotalFramesRendered = 0; // an increasing count
        static void UpdateFPS()
        {
            _framesRendered++;
            TotalFramesRendered++;
            if (_lastTime == null)
                _lastTime = DateTime.Now;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                // one second has elapsed 

                
                Fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }
           
        }
    }


    
}

