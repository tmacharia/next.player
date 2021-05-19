using System;
using Newtonsoft.Json;

namespace Next.PCL.Online.Models
{
    public class TvMazeSeason : TvMazeShowInnerEntity
    {
        [JsonProperty("episodeOrder")]
        public int? TotalEpisodes { get; set; }
        [JsonProperty("premiereDate")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }
    }
}