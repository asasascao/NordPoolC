using Apache.NMS.Stomp.Protocol;
using CaoNC.System;
using CaoNC.System.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NordPoolC.Extensions;
using NordPoolC.Logger;
using NordPoolC.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Subscription
{
    public class StompSubscription<TValue> : Subscription, ISubscription<TValue>
    {
        private readonly Channel<ReceivedMessage<IReadOnlyCollection<TValue>>> _channel;

        public ChannelReader<ReceivedMessage<IReadOnlyCollection<TValue>>> OutputChannel => _channel.Reader;

        public StompSubscription(
            string id,
            string type,
            string destination,
            Channel<ReceivedMessage<IReadOnlyCollection<TValue>>> channel)
            : base(id, type, destination)
        {
            _channel = channel;
        }

        public override void OnMessage(StompFrame frame, DateTimeOffset timestamp)
        {
            var contentBytes = new ReadOnlySpan<byte>(frame.Content);

            try
            {
                var data = JsonConvert.DeserializeObject<IReadOnlyCollection<TValue>>(Encoding.UTF8.GetString(frame.Content));
                if (data is null)
                {
                    return;
                }

                var message = new ReceivedMessage<IReadOnlyCollection<TValue>>(data, timestamp, frame.IsSnapshot(), frame.GetPublishingMode());

                if (_channel.Writer.TryWrite(message))
                {
                    return;
                }

                LogFactory.Instance.Warning(string.Format("[SubscriptionId:{0}][Destination:{1}] Channel for the subscription has reached its maximum capacity", Id, Destination));

                _channel.Writer.WriteAsync(message).AsTask()
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                LogFactory.Instance.Exception(e, "An error on web socket message handling");
                _channel.Writer.Complete(e);
            }
        }

        public sealed override void Close()
        {
            _channel.Writer.Complete();
        }
    }
}
