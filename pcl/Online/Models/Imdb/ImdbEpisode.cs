using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbEpisode : NamedEntity
    {
        public ImdbEpisode()
        {
            Rating = new ImdbRating();
            Cast = new List<ImdbCast>();
        }
        public int Number { get; set; }
        public string Notation { get; set; }
        public virtual Uri Url { get; set; }
        public virtual string ImdbId { get; set; }

        [JsonProperty("name")]
        public override string Name { get; set; }
        [JsonProperty("image")]
        [JsonConverter(typeof(StringToUriConverter))]
        public virtual Uri Poster { get; set; }
        [JsonProperty("description")]
        public virtual string Plot { get; set; }
        [JsonProperty("timeRequired")]
        [JsonConverter(typeof(StringToRuntimeConverter))]
        public virtual int? Runtime { get; set; }
        [JsonProperty("datePublished")]
        [JsonConverter(typeof(StringToDateTimeConverter))]
        public virtual DateTime? ReleaseDate { get; set; }
        [JsonProperty("aggregateRating")]
        public ImdbRating Rating { get; set; }
        [JsonProperty("actor")]
        public List<ImdbCast> Cast { get; set; }


        public override string ToString()
        {
            return string.Format("{0}, {1} - {2}, {3}, {4:dd MMM yyyy}" +
                "\n=> {5}" +
                "\n\t{6}" +
                "\n\t{7}",
                Number, Notation, Name, ImdbId, ReleaseDate,
                Rating.ToString(),
                Url,
                Poster);
        }
    }
}