using System;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Next.PCL.Extensions;

namespace Next.PCL.Converters
{
    internal class StringToDateTimeConverter : JsonConverter
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
                    s = s.Trim();
                    if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        return date;
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