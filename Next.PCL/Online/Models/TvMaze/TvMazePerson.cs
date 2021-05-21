using System;
using Newtonsoft.Json;
using Next.PCL.Converters;
using Next.PCL.Entities;
using Next.PCL.Enums;

namespace Next.PCL.Online.Models
{
    public class TvMazePerson : TvMazeEntity, IGender
    {
        [JsonProperty("gender")]
        [JsonConverter(typeof(StringToGenderConverter))]
        public Gender Gender { get; set; }
        [JsonProperty("birthday")]
        public DateTime? Birthday { get; set; }
        [JsonProperty("country")]
        public TvMazeCountry Country { get; set; }
    }
}