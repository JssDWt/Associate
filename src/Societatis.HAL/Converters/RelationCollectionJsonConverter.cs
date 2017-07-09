namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.Misc;

    public class RelationCollectionJsonConverter : JsonConverter
    {
        public RelationCollectionJsonConverter()
            : base()
        {
            
        }

        public override bool CanConvert(Type objectType)
        {
            objectType.ThrowIfNull(nameof(objectType));
            return typeof(RelationCollection<>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.ThrowIfNull(nameof(writer));
            value.ThrowIfNull(nameof(value));

            var valueType = value.GetType();
            bool isRelationCollection = valueType.GetGenericTypeDefinition() == typeof(RelationCollection<>);

            if (!isRelationCollection)
            {
                throw new ArgumentException($"Cannot write type '{valueType.FullName}', because it is not a '{typeof(RelationCollection<>).FullName}'");
            }

            var valueTypeInfo = valueType.GetTypeInfo();
            var jsonObject = new JObject();
            IEnumerable<string> singles = (IEnumerable<string>)valueTypeInfo.GetDeclaredProperty(nameof(RelationCollection<dynamic>.SingleRelations))
                                                       .GetMethod
                                                       .Invoke(value, null);
            foreach (var relation in (IEnumerable<string>)valueTypeInfo.GetDeclaredProperty(nameof(RelationCollection<dynamic>.Relations))
                                                                       .GetMethod
                                                                       .Invoke(value, null))
            {
                var getMethod = valueTypeInfo.GetDeclaredMethods(nameof(RelationCollection<dynamic>.Get))
                             .Single(m => 
                                {
                                    var parameters = m.GetParameters();
                                    return parameters.Length == 1
                                        && parameters.First().ParameterType == typeof(string);
                                });
                
                
                var objects = (IEnumerable<object>)getMethod.Invoke(value, new object[] {relation});

                JToken current = null;

                if (singles.Contains(relation))
                {
                    current = JToken.FromObject(objects.Single());
                }
                else
                {
                    current = JArray.FromObject(objects);
                }

                jsonObject.Add(relation, current);
            }

            jsonObject.WriteTo(writer);
        }
    }
}