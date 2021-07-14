using System;
using Newtonsoft.Json;
using Next.PCL.Enums;
using Next.PCL.Extensions;

namespace Next.PCL.Online.Models
{
    public class TvMazeTinyImage
    {
        [JsonProperty("medium")]
        public Uri Medium { get; set; }
        [JsonProperty("original")]
        public Uri Original { get; set; }
    }
    public class TvMazeImage
    {
        [JsonProperty("type")]
        public string __type { get; set; }
        public MetaImageType ImgType => __type.ParseToMetaImageType();

        [JsonProperty("main")]
        public bool Main { get; set; }
        [JsonProperty("resolutions")]
        public TvMazeImageSizes Sizes { get; set; }
    }
    public class TvMazeImageSizes
    {
        [JsonProperty("original")]
        public TvMazeImageUrl Original { get; set; }
        [JsonProperty("medium")]
        public TvMazeImageUrl Medium { get; set; }
    }
    public class TvMazeImageUrl
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }
        [JsonProperty("width")]
        public ushort Width { get; set; }
        [JsonProperty("height")]
        public ushort Height { get; set; }
    }
}