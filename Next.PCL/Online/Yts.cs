using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models.Yts;

namespace Next.PCL.Online
{
    public class Yts : BaseOnline
    {
        internal async Task<TResponse> RequestAsync<TResponse>(string route, CancellationToken token = default) 
            where TResponse : class
        {
            if (!route.StartsWith("/"))
                route = $"/{route}";

            Uri url = new Uri(string.Format("{0}{1}", SiteUrls.YTS, route));

            string json = await GetAsync(url, token);
            if (json.IsValid())
            {
                var mx = json.DeserializeTo<BaseYtsResponse<TResponse>>();
                if (!mx.IsSuccess)
                    throw new OnlineException(mx.StatusMessage);
                return mx.Data;
            }
            return null;
        }
    }
}