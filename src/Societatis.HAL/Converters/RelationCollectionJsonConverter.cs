namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.Misc;

    /// <summary>
    /// Json converter for HAL relation collections.
    /// </summary>
    public class RelationCollectionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationCollectionJsonConverter" /> class.
        /// </summary>
        public RelationCollectionJsonConverter()
            : base()
        {
            
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            bool result = false;

            if (objectType != null)
            {
                result = objectType.IsOfGenericType(typeof(IRelationCollection<>));
            }

            return result;
            
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!this.CanConvert(objectType))
            {
                throw new ArgumentException($"Cannot read as type '{objectType?.FullName ?? "[NULL]"}', because it is not a '{typeof(IRelationCollection<>).FullName}'");
            }

            reader.ThrowIfNull(nameof(reader));
            serializer.ThrowIfNull(nameof(serializer));

            Type[] parameterTypes = objectType.GetGenericParameterTypes(typeof(IRelationCollection<>));
            if (parameterTypes == null
                || parameterTypes.Length != 1)
            {
                throw new ArgumentException("The specified object type does not meet the constraints needed to deserialize it as a relationcollection.");
            }

            Type parameterType = parameterTypes.Single();
            Type enumerableParameterType =  typeof(IEnumerable<>).MakeGenericType(parameterType);
            while (reader.TokenType == JsonToken.None)
            {
                // Advance to something useful.
                reader.Read();
            }

            // Expecting an object here.
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException("Expected start object.");
            }

            var instance = Activator.CreateInstance(objectType);
            ICollection<string> singleRelations = (ICollection<string>)typeof(IRelationCollection<>).GetRuntimeProperty(nameof(IRelationCollection.SingleRelations)).GetMethod.Invoke(instance, new object[0]);
            var addMethod = typeof(IRelationCollection<>).GetRuntimeMethod(nameof(IRelationCollection<dynamic>.Add), new Type[] { typeof(string), parameterType });
            var addMultipleMethod = typeof(IRelationCollection<>).GetRuntimeMethod(nameof(IRelationCollection<dynamic>.Add), new Type[] { typeof(string), enumerableParameterType });
            int level = 1;
            while (level > 0 && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        level++;
                        break;
                    case JsonToken.EndObject:
                        level--;
                        break;
                    case JsonToken.PropertyName:
                        if (level == 1)
                        {
                            string relation = (string)reader.Value;
                            reader.Read();
                            if (reader.TokenType == JsonToken.StartArray)
                            {
                                var items = serializer.Deserialize(reader, enumerableParameterType);
                                addMultipleMethod.Invoke(instance, new object[] { relation, items });
                            }
                            else
                            {
                                var item = serializer.Deserialize(reader, parameterType);
                                addMethod.Invoke(instance, new object[] {relation, item });
                                singleRelations.Add(relation);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return instance;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;

            writer.ThrowIfNull(nameof(writer));
            serializer.ThrowIfNull(nameof(serializer));

            var valueType = value.GetType();

            if (!this.CanConvert(valueType))
            {
                throw new ArgumentException($"Cannot write type '{valueType.FullName}', because it is not a '{typeof(IRelationCollection<>).FullName}'");
            }

            var valueTypeInfo = valueType.GetTypeInfo();

            // Get single relations
            IEnumerable<string> singles = (IEnumerable<string>) valueType.GetRuntimeProperty(nameof(RelationCollection<dynamic>.SingleRelations))
                                                                         .GetMethod
                                                                         .Invoke(value, null);

            // Get relations
            IEnumerable<string> relations = (IEnumerable<string>) valueType.GetRuntimeProperty(nameof(RelationCollection<dynamic>.Relations))
                                                                           .GetMethod
                                                                           .Invoke(value, null);
            
            writer.WriteStartObject();

            // Loop all relations
            foreach (var relation in relations)
            {
                // Get the items in the current relation.
                var getMethod = valueType.GetRuntimeMethod(nameof(RelationCollection<dynamic>.Get), new Type[] { typeof(string) });              
                var items = (IEnumerable<object>)getMethod.Invoke(value, new object[] {relation});

                writer.WritePropertyName(relation);

                if (singles.Contains(relation))
                {
                    serializer.Serialize(writer, items.FirstOrDefault());
                }
                else
                {
                    serializer.Serialize(writer, items);
                }
            }

            writer.WriteEndObject();
        }
    }
}