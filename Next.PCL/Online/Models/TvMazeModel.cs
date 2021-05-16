using System;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;

namespace Next.PCL.Online.Models
{
    public class TvMazeModel : BaseOnlineModel
    {
        public TvMazeModel()
        {
            Source = MetaSource.TVMAZE;
        }
        [JsonProperty("id")]
        public virtual int Id { get; set; }
        [JsonProperty("name")]
        public override string Name { get; set; }
        [JsonProperty("url")]
        public override Uri Url { get; set; }
        [JsonProperty("runtime")]
        public override int? Runtime { get; set; }
        [JsonProperty("summary")]
        public override string Plot { get; set; }
        [JsonProperty("premiered")]
        public override DateTime? ReleaseDate { get; set; }
        public override string ImdbId 
        {
            get
            {
                if (Externals != null)
                    return Externals.ImdbId;
                return base.ImdbId;
            }
            set => base.ImdbId = value; 
        }
        public int? TvdbId => Externals?.TvdbId;


        public string Company => Network?.Name;
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("officialSite")]
        [JsonConverter(typeof(StringToUriConverter))]
        public Uri Website { get; set; }

        [JsonProperty("image")]
        public TvMazeTinyImage Image { get; set; }
        [JsonProperty("externals")]
        public TvMazeExternals Externals { get; set; }
        [JsonProperty("network")]
        internal TvMazeNetwork Network { get; set; }
    }
    public class TvMazeExternals
    {
        [JsonProperty("tvrage")]
        public int? Tvrage { get; set; }
        [JsonProperty("thetvdb")]
        public int? TvdbId { get; set; }
        [JsonProperty("imdb")]
        public string ImdbId { get; set; }
    }
    internal class TvMazeNetwork : INamedEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}