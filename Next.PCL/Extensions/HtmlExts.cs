using System;
using System.Collections;
using System.Collections.Generic;
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
        internal static bool TextEquals(this HtmlNode node, string value)
        {
            string s = node.ParseText();
            if (s.IsValid())
                return s.EqualsOIC(value);
            return false;
        }
        internal static bool TextContains(this HtmlNode node, string value)
        {
            string s = node.ParseText();
            if (s.IsValid())
                return s.Matches(value);
            return false;
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


        internal static string GetAttrib(this HtmlNode node, string name, string defaultValue = default)
        {
            if (node.ContainsAttrib(name))
                return node.GetAttributeValue(name, defaultValue);
            return defaultValue;
        }
        internal static IEnumerable<string> GetAllAttribs(this HtmlNodeCollection nodes, string name)
        {
            return nodes.Select(x => x.GetAttrib(name))
                        .Where(x => x.IsValid());
        }
        internal static HtmlNode FirstContainingAttrib(this HtmlNodeCollection nodes, string name)
        {
            return nodes.FirstOrDefault(x => x.ContainsAttrib(name));
        }
        internal static HtmlNode FirstContainingAttrib(this HtmlNodeCollection nodes, string name, string value)
        {
            return nodes.FirstOrDefault(x => x.ContainsAttrib(name, value));
        }
        internal static bool ContainsAttrib(this HtmlNode node, string name)
        {
            return node.HasAttribWhere(x => x.Name.EqualsOIC(name));
        }
        internal static bool ContainsAttrib(this HtmlNode node, string name, string value)
        {
            return node.HasAttribWhere(x => x.Name.EqualsOIC(name) && x.Value.EqualsOIC(value));
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