using System;

namespace NordPoolC.Exceptions
{
    public class SubscriptionFailedException : Exception
    {
        public SubscriptionFailedException(string message) :
            base(message)
        { }
    }
}
