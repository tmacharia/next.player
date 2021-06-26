using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public class TvMazeCountry : INamedEntity
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}