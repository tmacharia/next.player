using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Next.PCL.Online.Models;
using Serilog;

namespace Next.PCL.Online
{
    public class Omdb : BaseOnline
    {
        private readonly string API_KEY = "";

        public Omdb(string apiKey)
        {
            API_KEY = apiKey;
        }

        public async Task<OmdbModel> FindAsync(string imdbId = default, string title = default, CancellationToken token = default, bool fullPlot = false, string type = "movie", int? year = default)
        {
            if (!API_KEY.IsValid())
                throw new ApiKeyException();

            if (!title.IsValid() && !imdbId.IsValid())
                throw new ArgumentException("At least one param is required (title or imdbId)");

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
            string json = await GetAsync(url, token);
            if (json.IsValid())
            {
                if (json.StartsWith("{"))
                {
                    var model = json.DeserializeTo<OmdbModel>();
                    if (model.IsSuccess)
                        return model;
                }
                else
                {
                    Log.Warning("response json from OMDB does not start with '{' as expected.");
                }
            }
            return null;
        }
    }
}