namespace Societatis.HAL
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    /// <summary>
    /// Class providing extension methods for adding HAL to a library.
    /// </summary>
    public static class HALExtensions
    {
        // public static JsonSerializer AddHALConverters(this JsonSerializer serializer)
        // {
        //     serializer.Converters.Add(new ResourceJsonConverter());
        //     serializer.Converters.Add(new RelationCollectionJsonConverter());
        //     serializer.Converters.Add(new MapJsonConverter<ILink, Link>());

        //     // NOTE: No need to add Resource here, because in practise, a resource will always be a specific type.
        //     return serializer;
        // }

        // public static JsonSerializer ConfigureDeserializationMap<TSource, TDestination>(this JsonSerializer serializer)
        // {
        //     serializer.Converters.Add(new MapJsonConverter<TSource, TDestination>());
        //     return serializer;
        // }
    }
}