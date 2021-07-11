using System;
using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public abstract class TvMazeShowInnerEntity : INamedEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public Uri Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("image")]
        public TvMazeTinyImage Image { get; set; }
        [JsonProperty("summary")]
        public string Plot { get; set; }

        public Uri SmallPoster => Image?.Medium;
        public Uri LargePoster => Image?.Original;

        public override string ToString()
        {
            return string.Format("No. {0}", Number);
        }
    }
}