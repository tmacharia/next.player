using Common;
using HtmlAgilityPack;
using Next.PCL.Online;

namespace Next.PCL.Html
{
    public class BaseParser : BaseOnline
    {
        protected virtual HtmlDocument ConvertToHtmlDoc(string html)
        {
            if (html.IsValid())
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
            return null;
        }
    }
}