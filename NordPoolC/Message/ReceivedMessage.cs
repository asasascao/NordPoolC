using Microsoft.IO;
using NordPoolC.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Message
{
    public class ReceivedMessage<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// 时间片
        /// </summary>
        public DateTimeOffset Timestamp { get; private set; }

        /// <summary>
        /// 是否快照
        /// </summary>
        public bool IsSnapshot { get; private set; } = false;

        /// <summary>
        /// 发送方式
        /// </summary>
        public PublishingMode PublishingMode { get; private set; }

        public ReceivedMessage(T Data, DateTimeOffset Timestamp, bool IsSnapshot, PublishingMode PublishingMode)
        {
            this.Data = Data;
            this.Timestamp = Timestamp;
            this.IsSnapshot = IsSnapshot;
            this.PublishingMode = PublishingMode;
        }

        public static implicit operator T(ReceivedMessage<T> msg)
        {
            return msg.Data;
        }
    }

    public sealed class ReceivedMessage : IDisposable
    {
        private static readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        private readonly MemoryStream _messageStream;

        public ReceivedMessage()
        {
            _messageStream = MemoryStreamManager.GetStream(tag: "received-message-stream");
        }

        /// <summary>
        /// 是否sock
        /// </summary>
        /// <returns></returns>
        public bool IsSockJsStart() => Is(WebSocketMessages.SockJsStart);

        /// <summary>
        /// 是否心跳
        /// </summary>
        /// <returns></returns>
        public bool IsHeartBeat() => Is(WebSocketMessages.ServerHeartBeat);

        /// <summary>
        /// 是否断联
        /// </summary>
        /// <returns></returns>
        public bool IsDisconnectCode() => Is(WebSocketMessages.DisconnectCode);

        /// <summary>
        /// 是否连接指令
        /// </summary>
        /// <returns></returns>
        public bool IsConnectedCommand() => Is(WebSocketMessages.ConnectedPrefix, compareLength: false);

        /// <summary>
        /// 是否消息指令
        /// </summary>
        /// <returns></returns>
        public bool IsMessageCommand() => Is(WebSocketMessages.MessagePrefix, compareLength: false);

        /// <summary>
        /// 是否错误
        /// </summary>
        /// <returns></returns>
        public bool IsError() => Is(WebSocketMessages.ErrorPrefix, compareLength: false);

        /// <summary>
        /// 是否一致
        /// </summary>
        /// <param name="other"></param>
        /// <param name="compareLength"></param>
        /// <returns></returns>
        private bool Is(byte[] other, bool compareLength = true)
        {
            if (compareLength && other.Length != _messageStream.Length)
            {
                return false;
            }
            var otherS=_messageStream.ToArray();
            for (int i = 0; i < other.Length; i++)
            {
                if (otherS[i] != other[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="isLast"></param>
        public void Append(byte[] bytes, int offset, int length, bool isLast)
        {
            _messageStream.Write(bytes, offset, length);
            if (isLast)
            {
                _messageStream.Position = 0;
            }
        }

        /// <summary>
        /// 获取流
        /// </summary>
        /// <returns></returns>
        public Stream GetStream() => _messageStream;

        public void Dispose()
        {
            _messageStream.Dispose();
        }
    }
}
