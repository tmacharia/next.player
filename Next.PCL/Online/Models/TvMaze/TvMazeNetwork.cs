using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    internal class TvMazeNetwork : INamedEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}