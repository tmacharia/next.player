using System;

namespace Next.PCL.Exceptions
{
    public class OnlineException : NextPCLException
    {
        public OnlineException() :base()
        { }
        public OnlineException(string message) : base(message)
        { }
        public OnlineException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}