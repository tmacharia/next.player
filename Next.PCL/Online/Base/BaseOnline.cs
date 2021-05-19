using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Next.PCL.Exceptions;
using Serilog;

namespace Next.PCL.Online
{
    public class BaseOnline : IOnline
    {
        /// <summary></summary>
        /// <exception cref="OnlineException"/>
        internal async Task<string> GetAsync(Uri uri, CancellationToken token = default)
        {
            using (HttpClient client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, uri);
                var res = await client.SendAsync(req, HttpCompletionOption.ResponseContentRead, token);
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsStringAsync();
                else
                {
                    int code = (int)res.StatusCode;
                    if(code == 301 || code == 302)
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
        }
        internal async Task<bool> PingAsync(Uri uri, CancellationToken token = default)
        {
            using (HttpClient client=new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Head, uri);
                var res = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, token);
                return res.IsSuccessStatusCode;
            }
        }
    }
}