using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using HtmlAgilityPack;
using Next.PCL.Online;

namespace Next.PCL.Html
{
    public class BaseParser : BaseOnline
    {
        public BaseParser(IHttpOnlineClient httpOnlineClient)
            :base(httpOnlineClient)
        { }

        /// <summary>
        /// Calls the <paramref name="uri"/> and loads the results as a <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken">Cancellation cancellationToken.</param>
        /// <param name="useHtmlWeb4Download">Whether to use <see cref="HtmlWeb"/> for loading html from an online resource or the simple &amp; often quick get.</param>
        /// <returns>A <see cref="HtmlDocument"/></returns>
        protected virtual async Task<HtmlDocument> GetHtmlDocumentAsync(Uri uri, CancellationToken cancellationToken = default, bool useHtmlWeb4Download = false)
        {
            if (!useHtmlWeb4Download)
            {
                string html = await GetAsync(uri, cancellationToken);
                return ConvertToHtmlDoc(html);
            }
            else
            {
                var wb = new HtmlWeb
                {
                    //UsingCache = true,
                    //UsingCacheIfExists=true,
                    CaptureRedirect=true,
                };
                return await wb.LoadFromWebAsync(uri.OriginalString, cancellationToken);
            }
        }
        internal virtual HtmlDocument ConvertToHtmlDoc(string html)
        {
            if (html.IsValid())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
            return null;
        }
    }
}