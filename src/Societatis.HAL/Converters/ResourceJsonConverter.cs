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
            serializer.ThrowIfNull(nameof(serializer));

            var resource = value as IResource;
            if (resource == null)
            {
                throw new ArgumentException($"Cannot convert type {value.GetType().FullName}, because it is not an {typeof(IResource).FullName}.", nameof(value));
            }
            
            object data = null;
            
            if (value.GetType().IsOfGenericType(typeof(Resource<>))
            {
                // Data property of Resource<> cannot be null, so data is now never null.
                data = typeof(Resource<>).GetProperty(nameof(Resource<dynamic>.Data)).GetValue(value);
            }
            else
            {
                data = value;
            }
            
            Type dataType = data.GetType();
            JsonObjectContract contract = serializer.ContractResolver.ResolveContract(dataType) as JsonObjectContract;
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
            
            if (resource.Embedded != null && resource.Embedded.Count > 0)
            {
                writer.WritePropertyName("_embedded");
                serializer.Serialize(writer, resource.Embedded);
            }
            
            foreach (var property in contract.Properties)
            {
                // TODO: Verify this works for edge cases.
                if (property.ShouldSerialize(data))
                {
                    writer.WritePropertyName(property.PropertyName);
                    serializer.Serialize(writer, dataType.GetProperty(property.UnderlyingName).GetMethod.Invoke(data, null);
                }
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
            bool result = false;
            if (objectType != null)
            {
                result = typeof(IResource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            }
            
            return result;
        }
    }
}
