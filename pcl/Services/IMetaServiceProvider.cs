using Next.PCL.Online.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Next.PCL.Services
{
    public interface IMetaServiceProvider<TResponse>
        where TResponse : IBaseOnlineModel
    {
        Task<List<TResponse>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}