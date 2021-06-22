﻿using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class StringToBoolConverter : JsonConverter
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
                    s = s.Trim();
                    var r = s.ParseToBool();
                    if (r.HasValue)
                        return r.Value;
                }
            }
            return default(bool);
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}