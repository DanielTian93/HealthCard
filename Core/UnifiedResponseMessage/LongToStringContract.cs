using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Core.UnifiedResponseMessage
{
    public class LongToStringContract : JsonPrimitiveContract
    {
        public LongToStringContract(Type underlyingType) : base(underlyingType)
        {
            this.Converter = new LongToStringJsonConverter();
        }
    }
    public class LongToStringJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (typeof(long) == objectType || typeof(long?) == objectType)
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //throw new NotImplementedException();
            if (reader.Value == null)
            {
                return null;
            }
            //long val = 0;
            //long.TryParse(reader.Value.ToString(),out val);
            try
            {
                return long.Parse(reader.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Error converting value '{reader.Value.ToString()}' to type '{objectType.Name}'", ex);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }
    }
}
