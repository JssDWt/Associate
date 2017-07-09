namespace Societatis.HAL
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.Misc;

    public class ResourceJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.ThrowIfNull(nameof(writer));
            value.ThrowIfNull(nameof(value));

            if (!this.CanConvert(value.GetType()))
            {
                throw new ArgumentException($"Cannot convert type {value.GetType().Name}.", nameof(value));
            }

            var resource = (Resource)value;
            var jsonResource = JObject.FromObject(resource);
            if (value.IsInstanceOfGenericType(typeof(Resource<>)))
            {
                var actualValue = value.GetType().GetTypeInfo().GetDeclaredProperty(nameof(Resource<dynamic>.Value)).GetMethod.Invoke(value, new object[0]);
                var jsonValue = JObject.FromObject(actualValue);
                foreach (var property in jsonValue.Properties())
                {
                    jsonResource.Add(property.Name, property.Value);
                }
            }

            if (resource.Links.RelationCount == 0
                && resource.Links.Count == 0)
            {
                jsonResource.Property("_links").Remove();
            }
            
            if (resource.Embedded.RelationCount == 0 
                && resource.Embedded.Count == 0)
            {
                jsonResource.Property("_embedded").Remove();
            }

            // TODO: Add embedded resources
            jsonResource.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            objectType.ThrowIfNull(nameof(objectType));
            return typeof(Resource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}