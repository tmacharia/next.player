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

        [JsonProperty("bestRating")]
        public double Best { get; set; }
        [JsonProperty("worstRating")]
        public double Worst { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\nBest: {1:N1}, Worst: {2:N1}", base.ToString(), Best, Worst);
        }
    }
}