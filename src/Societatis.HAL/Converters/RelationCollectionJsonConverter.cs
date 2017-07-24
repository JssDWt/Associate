namespace Societatis.HAL.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Societatis.Misc;

    /// <summary>
    /// Json converter for HAL relation collections.
    /// </summary>
    public class RelationCollectionJsonConverter : JsonConverter
    {
        private Type linkType;
        private Type concreteType;

        private const string AddMethodName = nameof(IRelationCollection<dynamic>.Add);

        public RelationCollectionJsonConverter()
        {
            
        }

        public RelationCollectionJsonConverter(Type concreteType)
        {
            concreteType.ThrowIfNull(nameof(concreteType));
            var typeInfo = concreteType.GetTypeInfo();
            if (!concreteType.IsConcreteType())
            {
                throw new ArgumentException("Type should be a concrete type.", nameof(concreteType));
            }

            if (!concreteType.GetTypeInfo().DeclaredConstructors.Any(c => c.GetParameters()?.Length == 0))
            {
                throw new ArgumentException("Type should have a default constructor.", nameof(concreteType));
            }

            this.concreteType = concreteType;
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
                var objectTypeInfo = objectType.GetTypeInfo();
                result = objectTypeInfo.IsOfGenericType(typeof(IRelationCollection<>));

                if (result == true && this.concreteType != null)
                {
                    result = objectTypeInfo.IsAssignableFrom(this.concreteType.GetTypeInfo());
                }
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

            AdvanceToData(reader);

            Type[] parameterTypes = objectType.GetGenericParameterTypes(typeof(IRelationCollection<>));
            if (parameterTypes == null
                || parameterTypes.Length != 1)
            {
                throw new ArgumentException("The specified object type does not meet the constraints needed to deserialize it as a relationcollection.");
            }

            Type parameterType = parameterTypes.Single();
            Type enumerableParameterType = typeof(IEnumerable<>).MakeGenericType(parameterType);

            var instance = GetRelationCollectionInstance(objectType, existingValue, serializer);

            // var singleRelations = (ICollection<string>)SingleRelationsProperty.GetValue(instance);
            // var addMethod = objectType.GetRuntimeMethod(AddMethodName, new Type[] { typeof(string), parameterType });
            // var addMultipleMethod = objectType.GetRuntimeMethod(AddMethodName, new Type[] { typeof(string), enumerableParameterType });
            // int level = 1;
            // while (level > 0 && reader.Read())
            // {
            //     switch (reader.TokenType)
            //     {
            //         case JsonToken.StartObject:
            //             level++;
            //             break;
            //         case JsonToken.EndObject:
            //             level--;
            //             break;
            //         case JsonToken.PropertyName:
            //             if (level == 1)
            //             {
            //                 string relation = (string)reader.Value;
            //                 reader.Read();
            //                 if (reader.TokenType == JsonToken.StartArray)
            //                 {
            //                     var items = serializer.Deserialize(reader, enumerableParameterType);
            //                     addMultipleMethod.Invoke(instance, new object[] { relation, items });
            //                 }
            //                 else
            //                 {
            //                     var item = serializer.Deserialize(reader, parameterType);
            //                     addMethod.Invoke(instance, new object[] { relation, item });
            //                     singleRelations.Add(relation);
            //                 }
            //             }
            //             break;
            //         default:
            //             break;
            //     }
            // }

            return instance;
        }

        private object GetRelationCollectionInstance(Type objectType, object defaultValue, JsonSerializer serializer)
        {
            object instance = null;
            if (defaultValue != null && defaultValue.IsInstanceOfGenericType(typeof(IRelationCollection<>)))
            {
                instance = defaultValue;
            }
            else
            {
                JsonContract objectContract = null;
                if (this.concreteType == null)
                {
                    objectContract = serializer.ContractResolver.ResolveContract(objectType);
                }
                else
                {
                    objectContract = serializer.ContractResolver.ResolveContract(this.concreteType);
                }

                instance = objectContract?.DefaultCreator?.Invoke();
            }

            if (instance == null)
            {
                throw new JsonSerializationException($"Could not resolve the contract for type {objectType.Name}");
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
            writer.ThrowIfNull(nameof(writer));
            serializer.ThrowIfNull(nameof(serializer));

            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var valueType = value.GetType();
            if (!this.CanConvert(valueType))
            {
                throw new ArgumentException($"Cannot write type '{valueType.FullName}', because it is not a '{typeof(IRelationCollection<>).FullName}'");
            }

            var relationTypeInfo = typeof(IRelation<>).GetTypeInfo();
            var relationProperty = relationTypeInfo.GetDeclaredProperty(nameof(IRelation<dynamic>.Relation));
            var isSingularProperty = relationTypeInfo.GetDeclaredProperty(nameof(IRelation<dynamic>.IsSingular));
            var itemsProperty = relationTypeInfo.GetDeclaredProperty(nameof(IRelation<dynamic>.Items));

            writer.WriteStartObject();

            foreach (var relation in (value as IEnumerable))
            {
                writer.WritePropertyName((string)relationProperty.GetValue(relation));

                var items = (IEnumerable)itemsProperty.GetValue(relation);
                if ((bool)isSingularProperty.GetValue(relation))
                {
                    var item = items.Cast<object>().SingleOrDefault();
                    serializer.Serialize(writer, item);
                }
                else
                {
                    serializer.Serialize(writer, items);
                }
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Advances the <see cref="JsonReader" /> to the start of the <see cref="IRelationCollection" /> object.
        /// </summary>
        /// <param name="reader">The reader to advance.</param>
        private static void AdvanceToData(JsonReader reader)
        {
            while (reader.TokenType != JsonToken.StartObject)
            {
                if (!reader.Read())
                {
                    throw new JsonSerializationException("Expected start object.");
                }
            }
        }
    }
}