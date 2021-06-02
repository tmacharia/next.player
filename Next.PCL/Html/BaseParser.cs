using Common;
using HtmlAgilityPack;

namespace Next.PCL.Html
{
    public class BaseParser
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