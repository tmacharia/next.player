using System;
using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public class TvMazeSeason : INamedEntity
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("url")]
        public Uri Url { get; set; }
        [JsonProperty("number")]
        public int? Number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("episodeOrder")]
        public int? TotalEpisodes { get; set; }
        [JsonProperty("premiereDate")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty("image")]
        public TvMazeTinyImage Image { get; set; }
        [JsonProperty("summary")]
        public string Plot { get; set; }

        public Uri SmallPoster => Image?.Medium;
        public Uri LargePoster => Image?.Original;
    }
}