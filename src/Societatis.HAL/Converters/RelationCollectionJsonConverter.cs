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
            if (value == null) return;

            writer.ThrowIfNull(nameof(writer));

            var valueType = value.GetType();

            if (!this.CanConvert(valueType))
            {
                throw new ArgumentException($"Cannot write type '{valueType.FullName}', because it is not a '{typeof(IRelationCollection<>).FullName}'");
            }

            var valueTypeInfo = valueType.GetTypeInfo();
            var jsonObject = new JObject();

            // Get single relations
            IEnumerable<string> singles = (IEnumerable<string>) valueType.GetRuntimeProperty(nameof(RelationCollection<dynamic>.SingleRelations))
                                                                         .GetMethod
                                                                         .Invoke(value, null);

            // Get relations
            IEnumerable<string> relations = (IEnumerable<string>) valueType.GetRuntimeProperty(nameof(RelationCollection<dynamic>.Relations))
                                                                           .GetMethod
                                                                           .Invoke(value, null);
            // Loop all relations
            foreach (var relation in relations)
            {
                // Get the items in the current relation.
                var getMethod = valueType.GetRuntimeMethod(nameof(RelationCollection<dynamic>.Get), new Type[] { typeof(string) });              
                var items = (IEnumerable<object>)getMethod.Invoke(value, new object[] {relation});

                JToken current = null;

                if (singles.Contains(relation))
                {
                    current = JToken.FromObject(items.FirstOrDefault());
                }
                else
                {
                    current = JArray.FromObject(items);
                }

                jsonObject.Add(relation, current);
            }

            jsonObject.WriteTo(writer);
        }
    }
}