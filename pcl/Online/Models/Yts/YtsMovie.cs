using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Online.Models.Yts
{
    public class YtsMovie : BaseOnlineModel
    {
        public YtsMovie()
        {
            Source = MetaSource.YTS_MX;
            Genres = new List<string>();
            Images = new List<MetaImageNx>();
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("imdb_code")]
        public override string ImdbId { get; set; }
        [JsonProperty("title")]
        public override string Name { get; set; }
        [JsonProperty("url")]
        public override Uri Url { get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("runtime")]
        public override int? Runtime { get; set; }
        [JsonProperty("rating")]
        public double Rating { get; set; }
        [JsonProperty("synopsis")]
        public override string Plot { get; set; }
        [JsonProperty("description_full")]
        public string DescriptionFull { get; set; }
        [JsonProperty("yt_trailer_code")]
        public string TrailerCode { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("mpa_rating")]
        public string MpaRating { get; set; }

        #region Images
        [JsonProperty("small_cover_image")]
        public Uri Poster_SM { get; set; }
        [JsonProperty("medium_cover_image")]
        public override Uri Poster { get; set; }
        [JsonProperty("large_cover_image")]
        public Uri Poster_LG { get; set; }

        [JsonProperty("background_image")]
        public Uri BackgroundImage { get; set; }
        [JsonProperty("background_image_original")]
        public Uri BackgroundImageOriginal { get; set; }

        [JsonProperty("large_screenshot_image1")]
        public Uri LargeScreenshotImage1 { get; set; }
        [JsonProperty("large_screenshot_image2")]
        public Uri LargeScreenshotImage2 { get; set; }
        [JsonProperty("large_screenshot_image3")]
        public Uri LargeScreenshotImage3 { get; set; }
        #endregion

        [JsonProperty("genres")]
        public override List<string> Genres { get; set; }
        public List<MetaImageNx> Images { get; set; }
    }
}