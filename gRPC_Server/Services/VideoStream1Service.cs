using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using VideoStream;

namespace gRPC_Server
{
    public class VideoStream1Implementation : VideoStream1.VideoStream1Base
    {
        public async override Task StreamVideo(IAsyncStreamReader<VideoFrame> requestStream, IServerStreamWriter<VideoFrame> responseStream, ServerCallContext context)
        {
            var readTask = Task.Run(async () =>
            {
                int counter = 1;
                while (await requestStream.MoveNext())
                {
                    try
                    {
                        Console.WriteLine($"Received frame {counter}");
                        byte[] p = new byte[requestStream.Current.Frame.Length];
                        requestStream.Current.Frame.CopyTo(p, 0);
                        p = AddWatermark(p);
                        ByteString byteString = ByteString.CopyFrom(p);
                        var frame = new VideoFrame() { Username = requestStream.Current.Username, Message = $"greetings from server frame {counter}", Frame = ByteString.CopyFrom(p) };
                        //var frame = new VideoFrame() { Username = requestStream.Current.Username, Message = $"greetings from server frame {counter}", Frame = requestStream.Current.Frame };
                        await responseStream.WriteAsync(frame);
                        counter++;
                    }
                    catch(Exception ex)
                    {

                    }
                }
            });

            await readTask;
        }

        private byte[] AddWatermark(byte[] frame)
        {
            byte[] watermarkedFrame = null;

            try
            {
                using (var watermarkedStream = new MemoryStream())
                using (var ms = new MemoryStream(frame))
                {
                    using (var img = Image.FromStream(ms))
                    {
                        using (var graphic = Graphics.FromImage(img))
                        {
                            var font = new Font(FontFamily.GenericSansSerif, 84, FontStyle.Bold, GraphicsUnit.Pixel);
                            var color = Color.FromArgb(128, 255, 255, 255);
                            var brush = new SolidBrush(color);
                            var point = new Point(img.Width - 550, img.Height - 100);

                            graphic.DrawString("gRPC stream", font, brush, point);
                            img.Save(watermarkedStream, ImageFormat.Jpeg);
                            watermarkedFrame = watermarkedStream.ToArray();
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }

            //protobuf does not accepts mull
            if (watermarkedFrame == null)
                watermarkedFrame = new byte[0];
            return watermarkedFrame;
        }
    }
}
