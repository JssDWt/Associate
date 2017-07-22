namespace Societatis.HAL
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Societatis.HAL.Streaming;
    using Societatis.Misc;

    public class ResourceJsonConverter : JsonConverter
    {
        private bool canRead = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceJsonConverter" /> class.
        /// </summary>
        public ResourceJsonConverter()
        {
            
        }

        public override bool CanRead => this.canRead;
        public override bool CanWrite => true;
        public override bool CanConvert(Type objectType)
        {
            bool result = false;
            if (objectType != null)
            {
                result = typeof(IResource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
            }
            
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IResource result = null;

            try
            {
                if (!this.CanConvert(objectType))
                {
                    throw new ArgumentException($"Cannot read as type '{objectType?.FullName ?? "[NULL]"}', because it is not a '{typeof(IResource).FullName}'");
                }

                reader.ThrowIfNull(nameof(reader));
                serializer.ThrowIfNull(nameof(serializer));

                var objectContract = serializer.ContractResolver.ResolveContract(objectType);
                // NOTE: This makes sure the current JsonConverter is not reused when deserializing the resource.
                this.canRead = false;
                if (objectType.IsOfGenericType(typeof(Resource<>)))
                {
                    var valueTypes = objectType.GetGenericParameterTypes(typeof(Resource<>));
                    if (valueTypes == null
                        ||valueTypes.Length != 1)
                    {
                        throw new ArgumentException("The specified object type does not meet the constraints needed to deserialize it as a Resource.");
                    }

                    var valueType = valueTypes.Single();
                    result = (IResource)objectContract.DefaultCreator();
                    
                    object data = null;
                    using (var resourceReader = new ResourceJsonReader(reader, serializer, result))
                    {
                        data = serializer.Deserialize(resourceReader, objectType);
                    }

                    if (data != null)
                    {
                        typeof(Resource<>).GetTypeInfo().GetDeclaredProperty(nameof(Resource<dynamic>.Data)).SetValue(result, data);
                    }
                }
                else
                {
                    result = (IResource)serializer.Deserialize(reader, objectType);
                }
            }
            finally
            {
                this.canRead = true;
            }

            return result;
        }

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
            Type resourceType = resource.GetType();
            JsonObjectContract resourceContract = serializer.ContractResolver.ResolveContract(resourceType) as JsonObjectContract;
            JsonObjectContract dataContract = null;
            Type dataType = null;
            if (resourceType.IsOfGenericType(typeof(Resource<>)))
            {
                // Data property of Resource<> cannot be null, so data is now never null.
                data = typeof(Resource<>).GetTypeInfo().GetDeclaredProperty(nameof(Resource<dynamic>.Data)).GetValue(resource);
                dataType = data.GetType();
                dataContract = serializer.ContractResolver.ResolveContract(dataType) as JsonObjectContract;
            }
            else
            {
                data = resource;
                dataType = resourceType;
                dataContract = resourceContract;
            }

            if (dataContract == null)
            {
                throw new JsonSerializationException("Could not resolve contract for the value to serialize.");
            }
            
            TypeInfo dataTypeInfo = dataType.GetTypeInfo();

            writer.WriteStartObject();

            if (resourceContract.Properties[Resource.LinksPropertyName]?.ShouldSerialize(resource) == true)
            {
                writer.WritePropertyName(Resource.LinksPropertyName);
                serializer.Serialize(writer, resource.Links);
            }
            
            if (resourceContract.Properties[Resource.EmbeddedPropertyName]?.ShouldSerialize(resource) == true)
            {
                writer.WritePropertyName(Resource.EmbeddedPropertyName);
                serializer.Serialize(writer, resource.Embedded);
            }
            
            foreach (var property in dataContract.Properties.Where(p => p.ShouldSerialize(data)))
            {
                writer.WritePropertyName(property.PropertyName);
                var propertyValue = dataType.GetRuntimeProperty(property.UnderlyingName).GetValue(data);
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }
    }
}
