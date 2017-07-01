namespace Societatis.HAL
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.Misc;

    public class ResourceConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            objectType.ThrowIfNull(nameof(objectType));
            return typeof(Resource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.ThrowIfNull(nameof(writer));
            throw new NotImplementedException();
        }
    }
}