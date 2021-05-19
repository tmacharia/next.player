using Newtonsoft.Json;

namespace Next.PCL.Online.Models
{
    public class TvMazeExternals
    {
        [JsonProperty("tvrage")]
        public int? Tvrage { get; set; }
        [JsonProperty("thetvdb")]
        public int? TvdbId { get; set; }
        [JsonProperty("imdb")]
        public string ImdbId { get; set; }
    }
}