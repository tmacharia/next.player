using Newtonsoft.Json;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models.Imdb
{
    public class ImdbRating : BaseRating
    {
        [JsonProperty("ratingCount")]
        public override int Votes { get; set; }
        [JsonProperty("ratingValue")]
        public override double Score { get; set; }
    }
}