using Next.PCL.Entities;
using Newtonsoft.Json;
using System;
using Next.PCL.Extensions;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbCast : NamedEntity
    {
        [JsonProperty("url")]
        protected string _UrlSuffix { get; private set; }
        [JsonProperty("name")]
        public override string Name { get; set; }

        public Uri Url => (SiteUrls.IMDB + _UrlSuffix).ParseToUri();
    }
}