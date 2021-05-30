using Common;
using Newtonsoft.Json;

namespace Next.PCL.Online.Models.Yts
{
    public class BaseYtsResponse<T> where T : class
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }

        public bool IsSuccess => Status.EqualsOIC("ok");
    }
}