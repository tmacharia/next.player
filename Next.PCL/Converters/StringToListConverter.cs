using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class StringToListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);
            if (jtoken.Type == JTokenType.String)
            {
                string s = jtoken.Value<string>();
                if (s.IsNotEmptyOr())
                {
                    if (s.Contains(','))
                        return s.SplitByAndTrim(",").ToList();
                    else
                        return new List<string>() { s };
                }
            }
            else if(jtoken.Type == JTokenType.Array)
            {
                return jtoken.Values<string>().ToList();
            }
            return new List<string>();
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    internal class NAStringConverterResolver : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);
            if (jtoken.Type == JTokenType.String)
            {
                string s = jtoken.Value<string>();
                if (s.IsNotEmptyOr())
                    return s;
            }
            return string.Empty;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}