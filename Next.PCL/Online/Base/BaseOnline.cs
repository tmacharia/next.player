using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Next.PCL.Exceptions;
using Serilog;

namespace Next.PCL.Online
{
    public abstract class BaseOnline : IOnline
    {
        internal static HttpClientHandler DefaulthttpClientHandler= new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            MaxAutomaticRedirections = 2,
            AutomaticDecompression = DecompressionMethods.GZip
        };
        private readonly HttpClientHandler _httpClientHandler;

        public BaseOnline(HttpClientHandler httpClientHandler = default)
        {
            _httpClientHandler = httpClientHandler ?? DefaulthttpClientHandler;
        }
        /// <summary></summary>
        /// <exception cref="OnlineException"/>
        internal async Task<string> GetAsync(Uri uri, CancellationToken token = default)
        {
            HttpClient client = GetHttpClient();

            var req = new HttpRequestMessage(HttpMethod.Get, uri);
            var res = await client.SendAsync(req, HttpCompletionOption.ResponseContentRead, token);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsStringAsync();
            else
            {
                int code = (int)res.StatusCode;
                if (code == 301 || code == 302)
                {
                    Log.Information("{0} Redirect to {1}", code, res.Headers.Location);
                    return await GetAsync(res.Headers.Location, token);
                }
                else
                {
                    Log.Error("http error {0} | {1}", res.StatusCode, res.ReasonPhrase);
                    throw new OnlineException(code, res.ReasonPhrase);
                }
            }
        }
        internal async Task<bool> PingAsync(Uri uri, CancellationToken token = default)
        {
            HttpClient client = GetHttpClient();
            var req = new HttpRequestMessage(HttpMethod.Head, uri);
            var res = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, token);
            return res.IsSuccessStatusCode;
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient(_httpClientHandler, false);
        }
    }
}