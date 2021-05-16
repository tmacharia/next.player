using System;

namespace Next.PCL.Exceptions
{
    public class ApiKeyException : NextPCLException
    {
        public ApiKeyException() : base()
        { }
        public ApiKeyException(string message) : base(message)
        { }
        public ApiKeyException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}