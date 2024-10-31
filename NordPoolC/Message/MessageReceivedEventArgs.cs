using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Message
{
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 消息
        /// </summary>
        public ReceivedMessage Message { get; set; }

        /// <summary>
        /// 时间片
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        public MessageReceivedEventArgs()
        {
            Message = new ReceivedMessage();
            Timestamp = new DateTimeOffset();
        }
    }
}
