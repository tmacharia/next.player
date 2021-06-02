using System;
using System.Xml.XPath;
using Common;
using HtmlAgilityPack;

namespace Next.PCL.Extensions
{
    internal static class HtmlExts
    {
        internal static string ParseText(this HtmlNode node)
        {
            if (node != null)
                return node.InnerText;
            return string.Empty;
        }
        internal static double? ParseDouble(this HtmlNode node)
        {
            string s = node.ParseText();
            if (s.IsValid() && double.TryParse(s, out double d))
            {
                return d;
            }
            return null;
        }
        internal static DateTime? ParseDateTime(this HtmlNode node)
        {
            string s = node.ParseText();
            if (s.IsValid() && DateTime.TryParse(s, out DateTime d))
            {
                return d;
            }
            return null;
        }
        
        
        internal static bool IsNotEmpty(this HtmlNodeCollection nodes)
        {
            return nodes != null && nodes.Count > 0;
        }

        internal static HtmlNode Find(this HtmlDocument node, string xpath)
        {
            return node.DocumentNode.SelectSingleNode(xpath);
        }
        internal static HtmlNodeCollection FindAll(this HtmlDocument node, string xpath)
        {
            return node.DocumentNode.SelectNodes(xpath);
        }
    }
}