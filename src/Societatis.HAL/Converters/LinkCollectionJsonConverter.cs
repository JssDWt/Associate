namespace Societatis.HAL.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public class LinkCollectionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Backing field for the <see cref="LinkType" /> property.
        /// </summary>
        private Type linkType;

        /// <summary>
        /// Contains the enumerable version of the <see cref="LinkType" /> property.
        /// </summary>
        private Type enumerableLinkType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollectionJsonConverter" /> class.
        /// </summary>
        public LinkCollectionJsonConverter()
        {
            this.LinkType = typeof(Link);
        }

        /// <summary>
        /// Gets or sets the type of link to use for deserialized links.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the provided type is not a concrete type.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided type does not implement <see cref="ILink" />.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided type does not have a default constructor.</exception>
        public Type LinkType
        {
            get => this.linkType;
            set
            {
                value.ThrowIfNull(nameof(value));
                var typeInfo = value.GetTypeInfo();
                if (!typeInfo.IsConcreteType())
                {
                    throw new ArgumentException("Type should be a concrete type.", nameof(value));
                }

                if (!typeof(ILink).GetTypeInfo().IsAssignableFrom(typeInfo))
                {
                    throw new ArgumentException($"Type must implement {nameof(ILink)}.", nameof(value));
                }

                // TODO: Support Uri constructor as well.
                var defaultConstructor = typeInfo.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor == null)
                {
                    throw new ArgumentException($"Type must have a default constructor.");
                }

                this.linkType = value;
                this.enumerableLinkType = typeof(IEnumerable<>).MakeGenericType(value);
            }
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;
        
        /// <summary>
        /// Provides a value indicating whether the specified object type can be converted with the current 
        /// <see cref="JsonConverter" />. Returns true if <see cref="IRelationCollection<ILink>" /> can be assigned from it.
        /// </summary>
        /// <param name="objectType">The type to convert.</param>
        /// <returns><c>true</c> if the specified type can be converted. Otherwise; <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            bool canConvert = false;

            if (objectType != null)
            {
                var objectTypeInfo = objectType.GetTypeInfo();
                canConvert = typeof(IRelationCollection<ILink>).GetTypeInfo().IsAssignableFrom(objectTypeInfo)
                    && objectTypeInfo.IsConcreteType();
            }

            return canConvert;
        }

        /// <summary>
        /// Reads the HAL relation collection object from json and populates a relation collection of links with them.
        /// </summary>
        /// <param name="reader">The reader to read json with.</param>
        /// <param name="objectType">The type of the object to instantiate.</param>
        /// <param name="existingValue">An existing instance of <see cref="IRelationCollection<ILink>" />.</param>
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
            var objectTypeInfo = objectType.GetTypeInfo();
            if (!objectTypeInfo.IsConcreteType())
            {
                throw new ArgumentException("Cannot read json into a non-concrete type.", nameof(objectType));
            }

            var objectTypeContract = serializer.ContractResolver.ResolveContract(objectType);
            var linkCollection = (IRelationCollection<ILink>)(existingValue ?? objectTypeContract.DefaultCreator());

            AdvanceToData(reader);
            int objectDepth = reader.Depth;
            while (reader.Depth >= objectDepth 
                && reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName
                    && reader.Depth == objectDepth)
                {
                    PopulateRelation(linkCollection, reader, serializer);
                }
            }

            return linkCollection;
        }

        /// <summary>
        /// Populates the specified link collection with the relation property the reader is currently at.
        /// This method expects the reader to be pointed at a <see cref="JsonToken.PropertyName" /> inside the 
        /// relation collection object.
        /// </summary>
        /// <param name="linkCollection">The collection to populate the relation on.</param>
        /// <param name="reader">The reader to read the relation from.</param>
        /// <param name="serializer">Serializer to deserialize links inside the current relation.</param>
        private void PopulateRelation(IRelationCollection<ILink> linkCollection, JsonReader reader, JsonSerializer serializer)
        {
            string relation = (string)reader.Value;

            // Advance to the property value.
            while (reader.TokenType != JsonToken.StartArray 
                && reader.TokenType != JsonToken.StartObject
                && reader.Read())
            {
            }

            // Set the relation based on whether it is an array or a single item.
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    var links = (IEnumerable<ILink>)serializer.Deserialize(reader, this.enumerableLinkType);
                    linkCollection.Add(relation, links);
                    break;
                case JsonToken.StartObject:
                    var link = (ILink)serializer.Deserialize(reader, this.LinkType);

                    // Mark the relation as singular as well.
                    linkCollection.MarkSingular(relation);
                    linkCollection.Add(relation, link);
                    break;
                default: 
                    throw new JsonSerializationException(
                        $"Expecting {nameof(JsonToken.StartArray)} or {nameof(JsonToken.StartObject)} token " +
                        "as the start of a relation value.");
            }
        }
        
        /// <summary>
        /// Advances the reader to the start of the object.
        /// </summary>
        /// <param name="reader">The reader to advance.</param>
        private void AdvanceToData(JsonReader reader)
        {
            while (reader.TokenType != JsonToken.StartObject)
            {
                if (!reader.Read())
                {
                    throw new JsonSerializationException("Cannot find start of link collection.");
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}