﻿// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: VideoStream1.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VideoStream
{

    /// <summary>Holder for reflection information generated from VideoStream1.proto</summary>
    public static partial class VideoStream1Reflection
    {

        #region Descriptor
        /// <summary>File descriptor for VideoStream1.proto</summary>
        public static pbr::FileDescriptor Descriptor
        {
            get { return descriptor; }
        }
        private static pbr::FileDescriptor descriptor;

        static VideoStream1Reflection()
        {
            byte[] descriptorData = global::System.Convert.FromBase64String(
                string.Concat(
                  "ChJWaWRlb1N0cmVhbTEucHJvdG8SC1ZpZGVvU3RyZWFtIj4KClZpZGVvRnJh",
                  "bWUSEAoIdXNlcm5hbWUYASABKAkSDwoHbWVzc2FnZRgCIAEoCRINCgVmcmFt",
                  "ZRgDIAEoDDJTCgxWaWRlb1N0cmVhbTESQwoLU3RyZWFtVmlkZW8SFy5WaWRl",
                  "b1N0cmVhbS5WaWRlb0ZyYW1lGhcuVmlkZW9TdHJlYW0uVmlkZW9GcmFtZSgB",
                  "MAFiBnByb3RvMw=="));
            descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
                new pbr::FileDescriptor[] { },
                new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VideoStream.VideoFrame), global::VideoStream.VideoFrame.Parser, new[]{ "Username", "Message", "Frame" }, null, null, null)
                }));
        }
        #endregion

    }
    #region Messages
    /// <summary>
    /// The request message containing the user's name.
    /// </summary>
    public sealed partial class VideoFrame : pb::IMessage<VideoFrame>
    {
        private static readonly pb::MessageParser<VideoFrame> _parser = new pb::MessageParser<VideoFrame>(() => new VideoFrame());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<VideoFrame> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::VideoStream.VideoStream1Reflection.Descriptor.MessageTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public VideoFrame()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public VideoFrame(VideoFrame other) : this()
        {
            username_ = other.username_;
            message_ = other.message_;
            frame_ = other.frame_;
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public VideoFrame Clone()
        {
            return new VideoFrame(this);
        }

        /// <summary>Field number for the "username" field.</summary>
        public const int UsernameFieldNumber = 1;
        private string username_ = "";
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public string Username
        {
            get { return username_; }
            set
            {
                username_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        /// <summary>Field number for the "message" field.</summary>
        public const int MessageFieldNumber = 2;
        private string message_ = "";
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public string Message
        {
            get { return message_; }
            set
            {
                message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        /// <summary>Field number for the "frame" field.</summary>
        public const int FrameFieldNumber = 3;
        private pb::ByteString frame_ = pb::ByteString.Empty;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public pb::ByteString Frame
        {
            get { return frame_; }
            set
            {
                frame_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other)
        {
            return Equals(other as VideoFrame);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(VideoFrame other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (Username != other.Username) return false;
            if (Message != other.Message) return false;
            if (Frame != other.Frame) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Username.Length != 0) hash ^= Username.GetHashCode();
            if (Message.Length != 0) hash ^= Message.GetHashCode();
            if (Frame.Length != 0) hash ^= Frame.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output)
        {
            if (Username.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(Username);
            }
            if (Message.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Message);
            }
            if (Frame.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteBytes(Frame);
            }
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize()
        {
            int size = 0;
            if (Username.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeStringSize(Username);
            }
            if (Message.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeStringSize(Message);
            }
            if (Frame.Length != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeBytesSize(Frame);
            }
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(VideoFrame other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Username.Length != 0)
            {
                Username = other.Username;
            }
            if (other.Message.Length != 0)
            {
                Message = other.Message;
            }
            if (other.Frame.Length != 0)
            {
                Frame = other.Frame;
            }
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 10:
                        {
                            Username = input.ReadString();
                            break;
                        }
                    case 18:
                        {
                            Message = input.ReadString();
                            break;
                        }
                    case 26:
                        {
                            Frame = input.ReadBytes();
                            break;
                        }
                }
            }
        }

    }

    #endregion

}

#endregion Designer generated code
