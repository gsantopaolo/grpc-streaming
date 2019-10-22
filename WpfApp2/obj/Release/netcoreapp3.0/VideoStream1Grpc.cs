// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: VideoStream1.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace VideoStream {
  /// <summary>
  /// The greeting service definition.
  /// </summary>
  public static partial class VideoStream1
  {
    static readonly string __ServiceName = "VideoStream.VideoStream1";

    static readonly grpc::Marshaller<global::VideoStream.VideoFrame> __Marshaller_VideoStream_VideoFrame = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::VideoStream.VideoFrame.Parser.ParseFrom);

    static readonly grpc::Method<global::VideoStream.VideoFrame, global::VideoStream.VideoFrame> __Method_StreamVideo = new grpc::Method<global::VideoStream.VideoFrame, global::VideoStream.VideoFrame>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "StreamVideo",
        __Marshaller_VideoStream_VideoFrame,
        __Marshaller_VideoStream_VideoFrame);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::VideoStream.VideoStream1Reflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for VideoStream1</summary>
    public partial class VideoStream1Client : grpc::ClientBase<VideoStream1Client>
    {
      /// <summary>Creates a new client for VideoStream1</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public VideoStream1Client(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for VideoStream1 that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public VideoStream1Client(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected VideoStream1Client() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected VideoStream1Client(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncDuplexStreamingCall<global::VideoStream.VideoFrame, global::VideoStream.VideoFrame> StreamVideo(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return StreamVideo(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Sends a greeting
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncDuplexStreamingCall<global::VideoStream.VideoFrame, global::VideoStream.VideoFrame> StreamVideo(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_StreamVideo, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override VideoStream1Client NewInstance(ClientBaseConfiguration configuration)
      {
        return new VideoStream1Client(configuration);
      }
    }

  }
}
#endregion
