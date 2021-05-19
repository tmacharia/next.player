using System;
using Next.PCL.Entities;

namespace Next.PCL.Exceptions
{
    public class ApiRateLimitException : NextPCLException
    {
        public MetaSource ApiTarget { get; }

        public ApiRateLimitException(MetaSource apiTarget) : base()
        {
            ApiTarget = apiTarget;
        }
        public ApiRateLimitException(MetaSource apiTarget, string message) : base(message)
        {
            ApiTarget = apiTarget;
        }
        public ApiRateLimitException(MetaSource apiTarget, string message, Exception innerEx) 
            : base(message, innerEx)
        {
            ApiTarget = apiTarget;
        }
    }
}