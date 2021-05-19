using System;

namespace Next.PCL.Exceptions
{
    public class NextPCLException : Exception
    {
        public NextPCLException() : base()
        { }
        public NextPCLException(string message) : base(message)
        { }
        public NextPCLException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}