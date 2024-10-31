using Apache.NMS.Stomp.Protocol;
using CaoNC.System.Channels;
using NordPoolC.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Subscription
{
    public interface ISubscription<TValue> : ISubscription
    {
        ChannelReader<ReceivedMessage<IReadOnlyCollection<TValue>>> OutputChannel { get; }
    }

    public interface ISubscription
    {
        string Id { get; }
        string Type { get; }
        string Destination { get; }
    }

    public abstract class Subscription
    {
        public string Id { get; }
        public string Type { get; }
        public string Destination { get; }

        protected Subscription(string id, string type, string destination)
        {
            Id = id;
            Type = type;
            Destination = destination;
        }

        public abstract void OnMessage(StompFrame frame, DateTimeOffset timestamp);
        public abstract void Close();
    }
}
