using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Next.PCL.Extensions
{
    internal static class TvdbExts
    {
        internal static List<Uri> GetArtworksOfType(this HtmlDocument doc, string imageType)
        {
            return doc.FindAll($"//a[@rel='artwork_{imageType}']")
                      ?.GetAllAttribs("href")
                      .Select(x => x.ParseToUri())
                      .Where(x => x != null)
                      .ToList() 
                      ?? new List<Uri>();
        }
    }
}