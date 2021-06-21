using Common;
using HtmlAgilityPack;
using Next.PCL.Entities;
using Next.PCL.Enums;
using Next.PCL.Exceptions;
using Next.PCL.Extensions;
using Next.PCL.Html;

namespace Next.PCL.Html
{
    internal static class SpecializedHtmlExts
    {
        internal static MetaUrl ParseToMetaUrl(this HtmlNode node, MetaSource metaSource)
        {
            if (node != null)
            {
                if (!node.Name.EqualsOIC("a"))
                    throw new ExpectationFailedException("a", node.Name);

                return node.GetHref().ParseToUri().ParseToMetaUrl(metaSource);
            }
            return null;
        }
        internal static Company ParseToCompany(this HtmlNode node, MetaSource metaSource, CompanyService companyType = CompanyService.Network)
        {
            if (node != null)
            {
                if (!node.Name.EqualsOIC("a"))
                    throw new ExpectationFailedException("a", node.Name);

                var model = new Company
                {
                    Name = node.ParseText(),
                    Service = companyType
                };
                model.Urls.Add(node.ParseToMetaUrl(metaSource));
                return model;
            }
            return null;
        }
    }
}