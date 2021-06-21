using System;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbTrailer : NamedEntity
    {
        [JsonProperty("name")]
        public override string Name { get; set; }
        [JsonProperty("description")]
        public string Plot { get; set; }
        [JsonProperty("thumbnailUrl")]
        [JsonConverter(typeof(StringToUriConverter))]
        public Uri Thumbnail { get; set; }
        [JsonProperty("embedUrl")]
        [JsonConverter(typeof(StringToUriConverter))]
        public Uri EmbedUrl { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, EmbedUrl);
        }
    }
}