using Grpc.Core;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        BackgroundWorker worker;
        VideoCapture capture;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            capture = new VideoCapture(1);
            // Frame image buffer
            using (Mat image = new Mat()) 
            {
                int interval = (int)(1000 / capture.Fps == 0 ? 30 : capture.Fps);

                while (true)
                {
                    if (capture.IsDisposed == false)
                        capture.Read(image);

                    if (image.Empty())
                        break;
                    //window.ShowImage(image);

                    bw.ReportProgress(0, image);
                    
                    if (bw.CancellationPending == true)
                    {
                        capture.Dispose();
                        break;
                    }

                    System.Threading.Thread.Sleep(interval);
                }
            }
            
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                Mat image = (Mat)e.UserState;
                picturebox.Source = MatToBitmapImage(image);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private BitmapImage MatToBitmapImage(Mat image)
        {
            Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg); 

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        private byte[] GetCameraFrame()
        {

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(picturebox.Source as BitmapSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //VideoCapture capture = new VideoCapture(1);
            //using (OpenCvSharp.Window window = new OpenCvSharp.Window("Camera"))
            //using (Mat image = new Mat()) // Frame image buffer
            //{
            //    // When the movie playback reaches end, Mat.data becomes NULL.
            //    while (true)
            //    {
            //        capture.Read(image); // same as cvQueryFrame
            //        if (image.Empty()) break;
            //        window.ShowImage(image);
            //        Cv2.WaitKey(30);
            //    }
            //}
            if (worker == null)
            {
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerAsync();
                await StartClient();
            }
            else
            {
                worker.CancelAsync();
                worker.Dispose();
                worker.DoWork -= new DoWorkEventHandler(worker_DoWork);
                worker.ProgressChanged -= new ProgressChangedEventHandler(worker_ProgressChanged);
                worker = null;
                picturebox.Source = null;
            }


        }

        private async Task StartClient()
        {
            // client
            var channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);

            var client = new VideoStream.VideoStream1.VideoStream1Client(channel);



            using (var call = client.StreamVideo())
            {

                // Read incoming messages in a background task

                var readTask = Task.Run(async () =>
                {
                    await foreach (var message in call.ResponseStream.ReadAllAsync())
                    {
                        
                    }
                });

                // Write outgoing messages until picturebox.Source 
                while (true)
                {
                    var frame = new VideoStream.VideoFrame() { Username = "username", Message = "hello" };
                    frame.Frame = Google.Protobuf.ByteString.CopyFrom(GetCameraFrame());
                    //frame.Frame.AddRange(requestStream.Current.Frame);
                    await call.RequestStream.WriteAsync(frame);
                }

                //// Finish call and report results
                //await call.RequestStream.CompleteAsync();
                //await readTask;

            }

            //Task.WaitAll(tasks.ToArray());
        }
    }
}
