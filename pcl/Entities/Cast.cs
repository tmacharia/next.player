using Newtonsoft.Json;
using System;

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
    public class ReviewComment : RootEntity<string>
    {
        public Uri Url { get; set; }
        public double? Score { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public new DateTime? Timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("{0:N0}/10 by {1}, {2}", Score.HasValue ? Score.Value.ToString() : "?", Author, Content.Substring(0, 10));
        }
    }
}