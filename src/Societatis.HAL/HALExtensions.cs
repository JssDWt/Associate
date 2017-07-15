using Newtonsoft.Json;

namespace Societatis.HAL
{
    public static class HALExtensions
    {
        public static JsonSerializer AddHALConverters(this JsonSerializer serializer)
        {
            serializer.Converters.Add(new ResourceJsonConverter());
            serializer.Converters.Add(new RelationCollectionJsonConverter());
            serializer.ConfigureDeserializationMap<ILink, Link>()
                      .ConfigureDeserializationMap<IRelationCollection<ILink>, LinkCollection>()
                      .ConfigureDeserializationMap<IResource, Resource>()
                      .ConfigureDeserializationMap<IRelationCollection<IResource>, ResourceCollection>();

            return serializer;
        }

        public static JsonSerializer ConfigureDeserializationMap<TSource, TDestination>(this JsonSerializer serializer)
        {
            serializer.Converters.Add(new MapJsonConverter<TSource, TDestination>());
            return serializer;
        }
    }
}