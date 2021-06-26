using Newtonsoft.Json;

namespace Next.PCL.Online.Models
{
    public class TvMazeSearchModel
    {
        [JsonProperty("score")]
        public double Score { get; set; }
        [JsonProperty("show")]
        public TvMazeModel Show { get; set; }
    }
}