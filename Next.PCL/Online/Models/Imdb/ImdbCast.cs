using Next.PCL.Entities;
using Newtonsoft.Json;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbCast : NamedEntity
    {
        [JsonProperty("name")]
        public override string Name { get; set; }
    }
}