using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models;
using Next.PCL.Services;

namespace Next.PCL.Online
{
    public class Omdb : BaseOnline, IMetaServiceProvider<OmdbModel>
    {
        private readonly string API_KEY = "";

        public Omdb(string apiKey, IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        {
            if (!apiKey.IsValid())
                throw new ApiKeyException("OMdb Api key is required.");

            API_KEY = apiKey;
        }

        public async Task<OmdbModel> FindAsync(string imdbId = default, string title = default, bool fullPlot = false, string type = "movie", int? year = default, CancellationToken cancellationToken = default)
        {
            if (!title.IsValid() && !imdbId.IsValid())
                throw new NextArgumentException("At least one param is required (title or imdbId)");

            StringBuilder sb = new StringBuilder();
            sb.Append(SiteUrls.OMDB);
            sb.AppendFormat("?apikey={0}", API_KEY);
            if (fullPlot)
                sb.Append("&plot=full");
            if (year.HasValue)
                sb.AppendFormat("&y={0}", year.Value);
            if (imdbId.IsValid())
                sb.AppendFormat("&i={0}", imdbId);
            else
            {
                sb.AppendFormat("&t={0}", title);
                sb.AppendFormat("&type={0}", type);
            }

            var url = new Uri(sb.ToString());
            string json;
            try
            {
                json = await GetAsync(url, cancellationToken);
            }
            catch (Exception ex)
            {
                if(ex is OnlineException one)
                {
                    if (one.ResponseMessage.IsValid())
                    {
                        var model = one.ResponseMessage.DeserializeTo<OmdbError>();
                        if (!model.IsSuccess)
                        {
                            throw new OnlineException(model.ErrorMessage);
                        }
                    }
                }
                throw;
            }
            if (json.IsValid())
            {
                if (json.IsValidJson())
                {
                    var model = json.DeserializeTo<OmdbModel>();
                    if (model.IsSuccess)
                        return model;
                }
            }
            return null;
        }

        public Task<List<OmdbModel>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}