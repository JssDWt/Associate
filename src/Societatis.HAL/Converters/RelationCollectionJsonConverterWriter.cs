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
    public class RelationCollectionJsonConverterWriter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

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
                    && objectTypeInfo.GetGenericParameterTypes(typeof(IRelationCollection<>)).SingleOrDefault() != null;
            }

            return canConvert;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
    }
}