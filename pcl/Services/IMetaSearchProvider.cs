using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Online.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Next.PCL.Services
{
    public interface IBaseMetaProvider
    {
        MetaSource Source { get; }
    }
    public interface IMetaSearchProvider<TResponse> : IBaseMetaProvider
        where TResponse : IBaseOnlineModel
    {
        Task<List<TResponse>> SearchAsync(string query, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default);
    }
    public interface IMetaReviewsProvider : IBaseMetaProvider
    {
        Task<List<ReviewComment>> GetReviewsAsync(string metaId, MetaType metaType = MetaType.TvShow, CancellationToken cancellationToken = default);
    }
    public interface IMetaCompaniesProvider : IBaseMetaProvider
    {
        Task<Company> GetCompanyAsync(string metaId, CancellationToken cancellationToken = default);
    }
}