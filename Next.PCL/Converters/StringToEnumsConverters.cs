using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Entities;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class StringToGenderConverter : JsonConverter
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
                    if (s.EqualsOIC("Male"))
                        return Gender.Male;
                    else if (s.EqualsOIC("Female"))
                        return Gender.Female;
                }
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
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.String)
            {
                string s = token.Value<string>();
                if (s.IsNotEmptyOr())
                {
                    if (s.Matches("Director"))
                        return Profession.Director;
                    else if (s.Matches("Writer"))
                        return Profession.Writer;
                    else if (s.Matches("Producer"))
                        return Profession.Producer;
                }
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
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.String)
            {
                string s = token.Value<string>();
                if (s.IsNotEmptyOr())
                {
                    if (s.EqualsOIC("end,ended", true))
                        return MetaStatus.Ended;
                    else if (s.EqualsOIC("airing"))
                        return MetaStatus.Airing;
                    else if (s.EqualsOIC("production,inproduction",true))
                        return MetaStatus.InProduction;
                }
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