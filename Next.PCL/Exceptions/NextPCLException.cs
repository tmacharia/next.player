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