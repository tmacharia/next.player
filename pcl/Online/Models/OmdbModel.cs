using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Online.Models
{
    public class OmdbModel : BaseOnlineModel, IImdbRating
    {
        public OmdbModel()
        {
            Source = MetaSource.OMDB;
            Genres = new List<string>();
            Actors = new List<string>();
            Directors = new List<string>();
            Writers = new List<string>();
            Countries = new List<string>();
            Languages = new List<string>();
            Companies = new List<string>();
        }
        [JsonProperty("imdbID")]
        public override string ImdbId { get; set; }
        [JsonProperty("Title")]
        public override string Name { get; set; }
        [JsonProperty("Runtime")]
        [JsonConverter(typeof(StringToRuntimeConverter))]
        public override int? Runtime { get; set; }
        [JsonProperty("Plot")]
        [JsonConverter(typeof(NAStringConverterResolver))]
        public override string Plot { get; set; }
        [JsonProperty("Poster")]
        [JsonConverter(typeof(StringToUriConverter))]
        public override Uri Poster { get; set; }
        [JsonProperty("Released")]
        [JsonConverter(typeof(StringToDateTimeConverter))]
        public override DateTime? ReleaseDate { get; set; }


        [JsonProperty("Language")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Languages { get; set; }
        [JsonProperty("Country")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Countries { get; set; }
        [JsonProperty("Production")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Companies { get; set; }
        [JsonProperty("Website")]
        [JsonConverter(typeof(StringToUriConverter))]
        public Uri Website { get; set; }


        [JsonProperty("Genre")]
        [JsonConverter(typeof(StringToListConverter))]
        public override List<string> Genres { get; set; }
        [JsonProperty("Actors")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Actors { get; set; }
        [JsonProperty("Director")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Directors { get; set; }
        [JsonProperty("Writer")]
        [JsonConverter(typeof(StringToListConverter))]
        public List<string> Writers { get; set; }


        [JsonProperty("Metascore")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int Metascore { get; set; }
        [JsonProperty("imdbRating")]
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double ImdbScore { get; set; }
        [JsonProperty("imdbVotes")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int ImdbVotes { get; set; }


        [JsonProperty("Response")]
        [JsonConverter(typeof(StringToBoolConverter))]
        public bool IsSuccess { get; set; }
    }
}