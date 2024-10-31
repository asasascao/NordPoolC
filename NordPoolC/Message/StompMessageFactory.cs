using Apache.NMS.Stomp.Protocol;
using NordPoolC.Commands;
using NordPoolC.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Message
{
    public static class StompMessageFactory
    {
        public static StompFrame ConnectionFrame(string authToken, long heartbeatOutgoingInterval)
        {
            return CreateFrame(ServiceCommands.Client.Connect, new Dictionary<string, string>
        {
            { Headers.Client.AcceptVersion, "1.2,1.1,1.0" },
            { Headers.Client.AuthorizationToken, authToken },
            { Headers.Heartbeat, $"0,{heartbeatOutgoingInterval}" }
        });
        }

        public static StompFrame SendFrame(string payload, string destination,
            string contentType = "application/json;charset=UTF-8")
        {
            return CreateFrame(ServiceCommands.Client.Send, new Dictionary<string, string>
        {
            { Headers.ContentType, contentType },
            { Headers.Destination, destination }
        }, payload);
        }

        public static StompFrame SubscribeFrame(string destination, string id)
        {
            return CreateFrame(ServiceCommands.Client.Subscribe, new Dictionary<string, string>
        {
            { Headers.Destination, destination },
            { Headers.Client.SubscriptionId, id }
        });
        }

        public static StompFrame Unsubscribe(string id)
        {
            return CreateFrame(ServiceCommands.Client.Unsubscribe, new Dictionary<string, string>
        {
            { Headers.Client.SubscriptionId, id }
        });
        }

        private static StompFrame CreateFrame(string command, Dictionary<string, string> headers, string payload = null)
        {
            var frame = new StompFrame(true) { Command = command };

            foreach (var header in headers)
            {
                frame.SetProperty(header.Key, header.Value);
            }

            if (payload != null)
            {
                var contentBytes = Encoding.UTF8.GetBytes(payload);

                frame.Content = contentBytes;
                frame.SetProperty(Headers.ContentLength, contentBytes.Length);
            }

            return frame;
        }
    }
}
