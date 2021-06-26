using System;

namespace Next.PCL.Exceptions
{
    public class NextArgumentException : NextPCLException
    {
        public NextArgumentException() : base()
        { }
        public NextArgumentException(string message) : base(message)
        { }
        public NextArgumentException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}