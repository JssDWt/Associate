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
            if (value == null) return;
            
            writer.ThrowIfNull(nameof(writer));

            if (!this.CanConvert(value.GetType()))
            {
                throw new ArgumentException($"Cannot convert type {value.GetType().Name}.", nameof(value));
            }

            var resource = (Resource)value;
            object data = null;
            Type dataType = null;
            JsonObjectContract contract = null;
            
            if (value.GetType().IsOfGenericType(typeof(Resource<>))
            {
                // TODO: set data datatype and contract for a generic resource.
            }
            else
            {
                data = value;
                dataType = value.GetType();
                contract = serializer.ContractResolver.ResolveContract(dataType) as JsonObjectContract;
            }
            
            if (contract == null)
            {
                throw new JsonSerializationException("Could not resolve contract for the value to serialize.");
            }
            
            writer.WriteStartObject();
            if (resource.Links != null && resource.Links.Count > 0)
            {
                writer.WritePropertyName("_links");
                serializer.Serialize(writer, resource.Links);
            }
            
            foreach (var property in contract.Properties)
            {
                if (property.ShouldSerialize(data))
                {
                    writer.WritePropertyName(property.PropertyName);
                    serializer.Serialize(writer, dataType.GetProperty(property.UnderlyingName).GetMethod.Invoke(data, null);
                }
            }
            
            if (resource.Embedded != null && resource.Embedded.Count > 0)
            {
                writer.WritePropertyName("_embedded");
                serializer.Serialize(writer, resource.Embedded);
            }
            
            writer.WriteEndObject();
            
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
