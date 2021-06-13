using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using HtmlAgilityPack;

namespace Next.PCL.Extensions
{
    internal static class HtmlExts
    {
        #region Parsing to DataType
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
        #endregion

        #region Attribute Related
        internal static string GetAttrib(this HtmlNode node, string name, string defaultValue = default)
        {
            return node.GetAttributeValue(name, defaultValue);
        }
        internal static IEnumerable<string> GetAllAttribs(this HtmlNodeCollection nodes, string name)
        {
            return nodes.Select(x => x.GetAttrib(name))
                        .Where(x => x.IsValid());
        }
        internal static IEnumerable<HtmlNode> WhereHasAttrib(this HtmlNodeCollection nodes, string name)
        {
            return nodes.Where(x => x.HasAttrib(name));
        }
        internal static HtmlNode FirstWithAttrib(this HtmlNodeCollection nodes, string name)
        {
            return nodes.FirstOrDefault(x => x.HasAttrib(name));
        }
        internal static HtmlNode FirstWithAttrib(this HtmlNodeCollection nodes, string name, string value)
        {
            return nodes.FirstOrDefault(x => x.HasAttrib(name, value));
        }
        internal static HtmlNode FirstContainingClass(this HtmlNodeCollection nodes, string value)
        {
            return nodes.FirstOrDefault(x => x.ContainsClass(value));
        }
        internal static bool HasAttrib(this HtmlNode node, string name)
        {
            return node.HasAttribWhere(x => x.Name.EqualsOIC(name));
        }
        internal static bool HasAttrib(this HtmlNode node, string name, string value)
        {
            return node.HasAttribWhere(x => x.Name.EqualsOIC(name) && x.Value.EqualsOIC(value));
        }
        internal static bool ContainsClass(this HtmlNode node, string value)
        {
            if (node != null && node.HasAttributes)
                return node.GetAttrib("class").Contains(value);
            return false;
        }
        internal static bool HasAttribWhere(this HtmlNode node, Func<HtmlAttribute,bool> predicate)
        {
            if (node != null && node.HasAttributes)
                return node.Attributes.Any(predicate);
            return false;
        }

        internal static string GetHref(this HtmlNode node) => node.GetAttrib("href", string.Empty);
        #endregion

        #region Single Node
        internal static bool Is(this HtmlNode node, string htmlTagName)
        {
            if (node != null && htmlTagName.IsValid())
                return node.Name.EqualsOIC(htmlTagName);
            return false;
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
        internal static HtmlNode ExtendFind(this HtmlNode node, string xpath)
        {
            if(node != null)
                return node.SelectSingleNode(node.CombineXPath(xpath));
            return null;
        }
        internal static HtmlNodeCollection ExtendFindAll(this HtmlNode node, string xpath)
        {
            if (node != null)
                return node.SelectNodes(node.CombineXPath(xpath));
            return null;
        }
        internal static string CombineXPath(this HtmlNode node, string xpath)
        {
            var sb = new StringBuilder();
            if(node != null && xpath.IsValid())
            {
                sb.Append(node.XPath);

                if (!xpath.StartsWith("/"))
                    sb.Append("/");

                sb.Append(xpath);
            }
            return sb.ToString();
        }
        internal static HtmlNode GetAdjacent(this HtmlNode node, string tagName, int maxTraversals = 2)
        {
            if (node != null)
            {
                if (node.NextSibling != null)
                {
                    if (node.NextSibling.Name.Equals(tagName))
                        return node.NextSibling;

                    if (maxTraversals > 0)
                        return node.NextSibling.GetAdjacent(tagName, maxTraversals - 1);
                }
            }
            return null;
        }
        #endregion

        #region Nodes Collection
        internal static HtmlNode WhereTextEquals(this HtmlNodeCollection nodes, string value)
        {
            return nodes.FirstOrDefault(x => x.TextEquals(value));
        }
        internal static HtmlNode WhereTextContains(this HtmlNodeCollection nodes, string value)
        {
            return nodes.FirstOrDefault(x => x.TextContains(value));
        }
        #endregion

        #region Html Document
        internal static HtmlNode Find(this HtmlDocument node, string xpath)
        {
            return node.DocumentNode.SelectSingleNode(xpath);
        }
        internal static HtmlNodeCollection FindAll(this HtmlDocument node, string xpath)
        {
            return node.DocumentNode.SelectNodes(xpath);
        }
        internal static HtmlNodeCollection FindByTag(this HtmlDocument node, string tagName)
        {
            return node.DocumentNode.SelectNodes($"//{tagName}");
        }
        #endregion
    }
}