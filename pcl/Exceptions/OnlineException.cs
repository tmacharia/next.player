namespace Next.PCL.Exceptions
{
    public class OnlineException : NextPCLException
    {
        public int StatusCode { get; }
        public string ReasonPhrase { get; }
        public string ResponseMessage { get; }
        public OnlineException() :base()
        { }
        public OnlineException(string message) : base(message)
        { }
        public OnlineException(int statusCode, string reasonPhrase) 
            : base(string.Format("http error {0} | {1}", statusCode, reasonPhrase))
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }
        public OnlineException(int statusCode, string reasonPhrase, string responseMessage)
            : base(string.Format("http error {0} | {1}", statusCode, reasonPhrase), new OnlineException(responseMessage))
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ResponseMessage = responseMessage;
        }
    }
}