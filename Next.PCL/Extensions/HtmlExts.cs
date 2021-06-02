using System;
using System.Linq;
using Common;
using HtmlAgilityPack;

namespace Next.PCL.Extensions
{
    internal static class HtmlExts
    {
        internal static string ParseText(this HtmlNode node, params char[] trimChars)
        {
            if (node != null)
            {
                return node.InnerText.Trim(trimChars);
            }
            return string.Empty;
        }
        internal static int? ParseInt(this HtmlNode node)
        {
            return node.ParseText().ParseToInt();
        }
        internal static double? ParseDouble(this HtmlNode node)
        {
            return node.ParseText().ParseToDouble();
        }
        internal static DateTime? ParseDateTime(this HtmlNode node)
        {
            return node.ParseText().ParseToDateTime();
        }


        internal static string GetAttrib(this HtmlNode node, string attributeName, string defaultValue = default)
        {
            if (node.ContainsAttribName(attributeName))
                return node.GetAttributeValue(attributeName, defaultValue);
            return defaultValue;
        }
        internal static bool ContainsAttribName(this HtmlNode node, string attributeName)
        {
            return node.HasAttribWhere(x => x.Name.EqualsOIC(attributeName));
        }
        internal static bool HasAttribWhere(this HtmlNode node, Func<HtmlAttribute,bool> predicate)
        {
            if (node != null && node.HasAttributes)
                return node.Attributes.Any(predicate);
            return false;
        }

        internal static string GetHref(this HtmlNode node)
        {
            return node.GetAttrib("href", string.Empty);
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