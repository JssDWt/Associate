namespace Societatis.HAL
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;

    public class MapJsonConverter<TSource, TDestination> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            bool result = false;

            if (objectType != null)
            {
                result = typeof(TSource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            }

            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(TDestination));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}