using System.Collections.Generic;
using Newtonsoft.Json;

namespace Next.PCL.Online.Models.Yts
{
    public class YtsMovieListResponse
    {
        public YtsMovieListResponse()
        {
            Movies = new List<YtsMovie>();
        }
        [JsonProperty("movie_count")]
        public int MovieCount { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }
        [JsonProperty("movies")]
        public List<YtsMovie> Movies { get; set; }
    }
}