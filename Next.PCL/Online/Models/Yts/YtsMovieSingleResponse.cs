using Newtonsoft.Json;

namespace Next.PCL.Online.Models.Yts
{
    public class YtsMovieSingleResponse
    {
        [JsonProperty("movie")]
        public YtsMovie Movie { get; set; }
    }
}