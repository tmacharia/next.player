using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbModel : BaseOnlineModel
    {
        public ImdbModel()
        {
            Source = MetaSource.IMDB;
            OtherSites = new List<MetaUrl>();
            ProductionCompanies = new List<Company>();
            ProductionCountries = new List<GeographicLocation>();
        }
        [JsonProperty("name")]
        public override string Name { get; set; }
        [JsonProperty("image")]
        [JsonConverter(typeof(StringToUriConverter))]
        public override Uri Poster { get; set; }
        [JsonProperty("datePublished")]
        [JsonConverter(typeof(StringToDateTimeConverter))]
        public override DateTime? ReleaseDate { get; set; }
        [JsonProperty("description")]
        public override string Plot { get; set; }

        [JsonProperty("aggregateRating")]
        public ImdbRating Rating { get; set; }
        [JsonProperty("trailer")]
        public ImdbTrailer Trailer { get; set; }
        public MetaRevenue Revenue { get; set; }

        [JsonProperty("genre")]
        [JsonConverter(typeof(StringToListConverter))]
        public override List<string> Genres { get; set; }
        public List<MetaUrl> OtherSites { get; set; }
        public List<Company> ProductionCompanies { get; set; }
        public List<GeographicLocation> ProductionCountries { get; set; }
    }
}