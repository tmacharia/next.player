using Newtonsoft.Json;

namespace Next.PCL.Entities
{
    public class Cast : Person
    {
        public int Order { get; set; }
        [JsonProperty(
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Character { get; set; }
    }
}