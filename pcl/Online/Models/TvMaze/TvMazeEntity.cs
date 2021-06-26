using System;
using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public abstract class TvMazeEntity : INamedEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public Uri Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("image")]
        public TvMazeTinyImage Image { get; set; }

        public Uri SmallPoster => Image?.Medium;
        public Uri LargePoster => Image?.Original;
    }
}