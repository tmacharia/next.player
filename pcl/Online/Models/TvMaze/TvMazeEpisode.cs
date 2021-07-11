using System;
using Newtonsoft.Json;

namespace Next.PCL.Online.Models
{
    public class TvMazeEpisode : TvMazeShowInnerEntity
    {
        [JsonProperty("season")]
        public int Season { get; set; }
        [JsonProperty("airdate")]
        public DateTime? AirDate { get; set; }
        [JsonProperty("airtime")]
        public TimeSpan? AirTime { get; set; }
        [JsonProperty("runtime")]
        public int Runtime { get; set; }

        public override string ToString()
        {
            return string.Format("S{0}E{1}, {2}", Season, Number, Name);
        }
    }
}