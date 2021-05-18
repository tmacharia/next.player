﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class OmdbRuntimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.String)
            {
                string s = token.Value<string>();
                if (s.IsNotEmptyOr())
                {
                    s = s.Split(' ').First().Trim();
                    if (int.TryParse(s, out int n))
                        return n;
                }
            }
            return null;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}