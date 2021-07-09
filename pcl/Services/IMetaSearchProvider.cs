using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Online.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Next.PCL.Services
{
    public interface IMetaSearchProvider<TResponse>
        where TResponse : IBaseOnlineModel
    {
        Task<List<TResponse>> SearchAsync(string query, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default);
    }
    public interface IMetaReviewsProvider
    {
        Task<List<ReviewComment>> GetReviewsAsync(string metaId, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default);
    }
}