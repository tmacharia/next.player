using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public class TvMazeCast
    {
        [JsonProperty("person")]
        public TvMazePerson Person { get; set; }
        [JsonProperty("character")]
        public TvMazeEntity Character { get; set; }
        [JsonProperty("self")]
        public bool Self { get; set; }
        [JsonProperty("voice")]
        public bool Voice { get; set; }
    }
    public class TvMazeCrew
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringToProfessionConverter))]
        public Profession Role { get; set; }
        [JsonProperty("person")]
        public TvMazePerson Person { get; set; }
    }
}