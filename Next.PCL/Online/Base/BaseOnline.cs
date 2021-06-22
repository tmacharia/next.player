using System;
using System.Threading;
using System.Threading.Tasks;

namespace Next.PCL.Online
{
    public abstract class BaseOnline : IOnline
    {
        private readonly IHttpOnlineClient _httpOnlineClient;

        public BaseOnline(IHttpOnlineClient httpOnlineClient)
        {
            _httpOnlineClient = httpOnlineClient;
        }

        public Task<string> GetAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _httpOnlineClient.GetAsync(uri, cancellationToken);
        }
        public Task<bool> PingAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            return _httpOnlineClient.PingAsync(uri, cancellationToken);
        }
    }
}