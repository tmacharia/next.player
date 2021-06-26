using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Enums;

namespace Next.PCL.Online.Models
{
    public class TvMazeModel : BaseOnlineModel
    {
        public TvMazeModel() :base()
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


        [JsonProperty("genres")]
        public override List<string> Genres { get; set; }
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

        public override string ToString()
        {
            return string.Format("{0}\nTvMazeId: {1}, TvdbId: {2}, TvRageId: {3}", base.ToString(), Id, TvdbId, Externals?.Tvrage);
        }
    }
}