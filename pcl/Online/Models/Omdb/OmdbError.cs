using Newtonsoft.Json;

namespace Next.PCL.Online.Models
{
    public class OmdbError
    {
        [JsonProperty("Response")]
        public bool IsSuccess { get; set; }
        [JsonProperty("Error")]
        public string ErrorMessage { get; set; }
    }
}