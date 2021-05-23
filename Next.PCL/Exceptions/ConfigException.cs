using System;

namespace Next.PCL.Exceptions
{
    public class ConfigException : NextPCLException
    {
        public ConfigException() : base()
        { }
        public ConfigException(string message) : base(message)
        { }
        public ConfigException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}