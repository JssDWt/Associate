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
    public class RelationCollectionJsonReader<T> : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        /// <summary>
        /// Provides a value indicating whether the specified object type can be converted with the current 
        /// <see cref="JsonConverter" />. Returns true if <see cref="IRelationCollection<T>" /> can be assigned from it.
        /// </summary>
        /// <param name="objectType">The type to convert.</param>
        /// <returns><c>true</c> if the specified type can be converted. Otherwise; <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            bool canConvert = false;

            if (objectType != null)
            {
                var objectTypeInfo = objectType.GetTypeInfo();
                canConvert = objectTypeInfo.IsOfGenericType(typeof(IRelationCollection<>))
                    && typeof(T).GetTypeInfo().IsAssignableFrom(
                        objectTypeInfo.GetGenericParameterTypes(typeof(IRelationCollection<>)).Single().GetTypeInfo())
                    && objectTypeInfo.IsConcreteType();
            }

            return canConvert;
        }

        /// <summary>
        /// Reads the HAL relation collection object from json and populates a relation collection of T with them.
        /// </summary>
        /// <param name="reader">The reader to read json with.</param>
        /// <param name="objectType">The type of the object to instantiate.</param>
        /// <param name="existingValue">An existing instance of <see cref="IRelationCollection<T>" />.</param>
        /// <param name="serializer">The serializer to deserialize subtypes with.</param>
        /// <returns>A instance of the specified object type, populated with relations from the specified json.</returns>
        /// <exception cref="ArgumentNullException">Thrown when reader, objectType or serializer is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided type is not a concrete type.</exception>
        /// <exception cref="JsonSerializationException">Thrown when the provided json cannot be deserialized.</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.ThrowIfNull(nameof(reader));
            serializer.ThrowIfNull(nameof(serializer));
            objectType.ThrowIfNull(nameof(objectType));
            if (!this.CanConvert(objectType))
            {
                throw new ArgumentException($"Cannot convert type {objectType?.Name ?? "NULL"}.", nameof(objectType));
            }

            var objectTypeContract = serializer.ContractResolver.ResolveContract(objectType);

            // TODO: Make this method so that T can be any implementation of T. As if it were covariant so to say.
            var relationCollection = (IRelationCollection<T>)(existingValue ?? objectTypeContract.DefaultCreator());

            AdvanceToData(reader);
            int objectDepth = reader.Depth;
            while (reader.Depth >= objectDepth 
                && reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName
                    && reader.Depth == objectDepth)
                {
                    PopulateRelation(relationCollection, reader, serializer);
                }
            }

            return relationCollection;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Populates the specified relation collection with the relation property the reader is currently at.
        /// This method expects the reader to be pointed at a <see cref="JsonToken.PropertyName" /> inside the 
        /// relation collection object.
        /// </summary>
        /// <param name="relationCollection">The collection to populate the relation on.</param>
        /// <param name="reader">The reader to read the relation from.</param>
        /// <param name="serializer">Serializer to deserialize relation items inside the current relation.</param>
        private void PopulateRelation(IRelationCollection<T> relationCollection, JsonReader reader, JsonSerializer serializer)
        {
            string relation = (string)reader.Value;

            // Advance to the property value.
            while (reader.TokenType != JsonToken.StartArray 
                && reader.TokenType != JsonToken.StartObject
                && reader.Read())
            {
            }

            Type relationType = this.GetTypeForRelation(relation);
            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(relationType);

            // Set the relation based on whether it is an array or a single item.
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    var relationItems = (IEnumerable<T>)serializer.Deserialize(reader, enumerableType);
                    relationCollection.Add(relation, relationItems);
                    break;
                case JsonToken.StartObject:
                    var relationItem = (T)serializer.Deserialize(reader, relationType);

                    // Mark the relation as singular as well.
                    relationCollection.MarkSingular(relation);
                    relationCollection.Add(relation, relationItem);
                    break;
                default: 
                    throw new JsonSerializationException(
                        $"Expecting {nameof(JsonToken.StartArray)} or {nameof(JsonToken.StartObject)} token " +
                        "as the start of a relation value.");
            }
        }

        /// <summary>
        /// Gets the type to deserialize objects from the specified relation into.
        /// Always returns the <see cref="LinkType" />.
        /// </summary>
        /// <param name="relation">The relation containing items.</param>
        /// <returns>The type of the objects in the relation.</returns>
        protected virtual Type GetTypeForRelation(string relation)
        {
            // Always return the linktype.
            return typeof(T);
        }
    }
}