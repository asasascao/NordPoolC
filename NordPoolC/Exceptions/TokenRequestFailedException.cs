using System;

namespace NordPoolC.Exceptions
{
    public class TokenRequestFailedException : Exception
    {
        public TokenRequestFailedException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
