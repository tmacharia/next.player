using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Enums;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class StringToMetaTypeConverter : JsonConverter
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
                    return s.ParseToMetaType();
            }
            return MetaType.Unknown;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    internal class StringToGenderConverter : JsonConverter
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
                    return s.ParseToGender();
            }
            return Gender.Unknown;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    internal class StringToProfessionConverter : JsonConverter
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
                    return s.ParseToProfession();
            }
            return Profession.Other;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    internal class StringToMetaStatusConverter : JsonConverter
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
                    return s.ParseToMetaStatus();
            }
            return MetaStatus.Released;
        }
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}