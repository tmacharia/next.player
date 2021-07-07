using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Next.PCL.Infra;
using Serilog;

namespace Next.PCL.Online
{
    public class HttpOnlineClient : IHttpOnlineClient
    {
        private static int httpClientUseCount = 0;
        private static HttpClient _httpClient;
        private static HttpClientHandler _httpClientHandler;

        public HttpOnlineClient()
        {
            _httpClientHandler = DefaulthttpClientHandler;
        }
        public HttpOnlineClient(HttpClientHandler httpClientHandler = default)
        {
            _httpClientHandler = httpClientHandler ?? DefaulthttpClientHandler;
        }

        internal static HttpClientHandler DefaulthttpClientHandler { get; private set; } = GetHttpClientHandler();

        public async Task<string> GetAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpClient client = GetHttpClient();

            var req = new HttpRequestMessage(HttpMethod.Get, uri);
            var res = await client.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsStringAsync();
            else
            {
                int code = (int)res.StatusCode;
                if (code == 301 || code == 302)
                {
                    Log.Information("{0} Redirect to {1}", code, res.Headers.Location);
                    return await GetAsync(res.Headers.Location, cancellationToken);
                }
                else
                {
                    string msg = await res.Content.ReadAsStringAsync();
                    Log.Error("http error {0} | {1}\n{2}", res.StatusCode, res.ReasonPhrase, msg);
                    throw new OnlineException(code, res.ReasonPhrase, msg);
                }
            }
        }
        public async Task<bool> PingAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpClient client = GetHttpClient();
            var req = new HttpRequestMessage(HttpMethod.Head, uri);
            var res = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return res.IsSuccessStatusCode;
        }

        private static HttpClient GetHttpClient()
        {
            if (_httpClient == null)
            {
                HttpClient httpClient = new HttpClient(_httpClientHandler, false);
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(GetRandomUserAgent());
                httpClient.DefaultRequestHeaders.CacheControl = GetCacheControl();
                httpClient.Timeout = TimeSpan.FromSeconds(15);
                _httpClient = httpClient;
            }
            if (httpClientUseCount >= 10)
            {
                _httpClient.DefaultRequestHeaders.UserAgent.Clear();
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(GetRandomUserAgent());
                httpClientUseCount = 0;
            }
            httpClientUseCount++;
            return _httpClient;
        }
        private static CacheControlHeaderValue GetCacheControl()
        {
            CacheControlHeaderValue cacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                NoCache = true,
                Private = false,
                MustRevalidate = true,
                MaxStale = true
                //MaxAge = TimeSpan.FromSeconds(5 * 60)
            };
            return cacheControl;
        }
        private static HttpClientHandler GetHttpClientHandler()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 2,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            return handler;
        }
        internal static string GetRandomUserAgent()
        {
            var agents = new string[]
            {
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.1.1 Safari/605.1.15",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:77.0) Gecko/20100101 Firefox/77.0",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:77.0) Gecko/20100101 Firefox/77.0",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36"
            };

            int k = Randomizer.Instance.Next(0, agents.Length);
            return agents[k];
        }
    }
}