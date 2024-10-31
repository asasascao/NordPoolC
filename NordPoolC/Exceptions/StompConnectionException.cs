using System;

namespace NordPoolC.Exceptions
{
    public class StompConnectionException : Exception
    {
        public StompConnectionException(string message) :
            base(message)
        { }
    }
}
