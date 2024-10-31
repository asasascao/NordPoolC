using Apache.NMS.Stomp.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NordPoolC.Connection;
using NordPoolC.Enums;
using NordPoolC.Message;
using NordPoolC.WebSockets;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Extensions
{
    public static class StompFrameExtensions
    {
        /// <summary>
        /// 是否快照
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>是否快照</returns>
        public static bool IsSnapshot(this StompFrame frame)
        {
            var isSnapshotString = false;
            if (frame.Properties.Contains(Headers.Server.IsSnapshot))
            {
                isSnapshotString=(bool)frame.Properties[Headers.Server.IsSnapshot];
            }
            return isSnapshotString && string.Equals(isSnapshotString, "true");
        }

        /// <summary>
        /// 获取目标路径
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>目标路径</returns>
        public static string GetDestination(this StompFrame frame)
        {
            var destination = "";
            if (frame.Properties.Contains(Headers.Destination))
            {
                destination = frame.Properties[Headers.Destination].ToString();
            }
            _ = destination;
            return destination;
        }

        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>序列号</returns>
        public static string GetSequenceNumber(this StompFrame frame)
        {
            var sequenceNumber = "";
            if (frame.Properties.Contains(Headers.Server.SequenceNumber))
            {
                sequenceNumber = frame.Properties[Headers.Server.SequenceNumber].ToString();
            }
            _ = sequenceNumber;
            return sequenceNumber;
        }

        /// <summary>
        /// 获取发送方式
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>发送方式</returns>
        public static PublishingMode GetPublishingMode(this StompFrame frame)
        {
            var dest = "";
            if (frame.Properties.Contains(Headers.Destination))
            {
                dest = frame.Properties[Headers.Destination].ToString();
            }

            return !string.IsNullOrWhiteSpace(dest) && dest.Contains("/streaming/")
                ? PublishingMode.STREAMING
                : PublishingMode.CONFLATED;
        }
        
        /// <summary>
        /// 获取发送时间片
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>发送时间片</returns>
        public static DateTimeOffset? GetSentAtTimestamp(this StompFrame frame)
        {
            var sentAtHeaderValue = "";
            if (frame.Properties.Contains(Headers.Server.SentAt))
            {
                sentAtHeaderValue = frame.Properties[Headers.Server.SentAt].ToString();
            }
            if(!string.IsNullOrWhiteSpace(sentAtHeaderValue) && long.TryParse(sentAtHeaderValue, out var sentAtMs))
            {
                return (new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).AddMilliseconds(sentAtMs);
            }
            return null;
        }

        /// <summary>
        /// 发送StompFrame
        /// </summary>
        /// <param name="connector">连接</param>
        /// <param name="frame">StompFrame</param>
        /// <param name="cancellationToken">取消token</param>
        /// <returns>void</returns>
        public static Task SendStompFrameAsync(this IConnecter connector, StompFrame frame, CancellationToken cancellationToken)
        {
            return connector.SendAsync(frame.ConvertToMessageBytes(), cancellationToken);
        }

        /// <summary>
        /// StompFrame 转 byte[]
        /// </summary>
        /// <param name="frame">StompFrame</param>
        /// <returns>byte[]</returns>
        public static byte[] ConvertToMessageBytes(this StompFrame frame)
        {
            var messageText = frame.ToMessageText();
            JArray ja = new JArray();
            ja.Add(messageText);
            var serializedJsonArray = ja.ToString();
            var messageBytes = Encoding.UTF8.GetBytes(serializedJsonArray);
            return messageBytes;
        }

        private static string ToMessageText(this StompFrame frame)
        {
            var bytes = FrameToBytes(frame);
            var stringOfBytes = Encoding.UTF8.GetString(bytes);

            return stringOfBytes;
        }

        private static byte[] FrameToBytes(StompFrame frame)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    frame.ToStream(bw);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// ReceivedMessage 转 
        /// </summary>
        /// <param name="message">接到的消息</param>
        /// <returns>StompFrame</returns>
        public static StompFrame ConvertToStompFrame(this ReceivedMessage message)
        {
            var messageStream = message.GetStream();
            //Remove the first char 'a' to get the json array
            messageStream.Seek(1, SeekOrigin.Begin);

            var frame = new StompFrame(true);
            using (var streamReader = new StreamReader(messageStream))
            {
                string streamStr=streamReader.ReadToEnd();
                var stompMessage = (new JsonSerializer()).Deserialize<string[]>(new JsonTextReader(new 
                    StringReader(streamStr))).ElementAt(0);
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(stompMessage)))
                {
                    using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        frame.FromStream(binaryReader);
                    }
                }
            }
            return frame;
        }
    }
}
