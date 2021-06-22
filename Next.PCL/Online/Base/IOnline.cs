using System;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Exceptions;

namespace Next.PCL.Online
{
    public interface IOnline
    {
        /// <summary></summary>
        /// <exception cref="OnlineException"/>
        Task<bool> PingAsync(Uri uri, CancellationToken cancellationToken = default);
        /// <summary></summary>
        /// <exception cref="OnlineException"/>
        Task<string> GetAsync(Uri uri, CancellationToken cancellationToken = default);
    }
    public interface IHttpOnlineClient : IOnline
    {

    }
}