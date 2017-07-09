namespace Societatis.HAL.Tests
{
    using System;
    using Newtonsoft.Json;
    using Societatis.HAL;
    using Xunit;

    public class RelationCollectionJsonConverterTests
    {
        public class CanConvertMethod
        {
            [Theory]
            [InlineData(typeof(IRelationCollection))]
            [InlineData(typeof(IRelationCollection<object>))]
            [InlineData(typeof(IRelationCollection<>))]
            [InlineData(typeof(RelationCollection<>))]
            [InlineData(typeof(RelationCollection<ILink>))]
            [InlineData(typeof(RelationCollection<Resource>))]
            [InlineData(typeof(LinkCollection))]
            [InlineData(typeof(ResourceCollection))]
            [InlineData(typeof(TestRelationCollection))]
            [InlineData(typeof(HighlyInheritedRelationCollection))]
            public void KnownTypes_ReturnTrue(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonConverter();
                bool result = converter.CanConvert(type);
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(object))]
            [InlineData(typeof(System.Collections.Generic.List<>))]
            public void UnknownTypes_ReturnFalse(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonConverter();
                bool result = converter.CanConvert(type);
                Assert.False(result);
            }
        }

        public class ReadJsonMethod
        {
            [Fact]
            public void SimpleLinkCollection()
            {
                JsonConverter converter = new RelationCollectionJsonConverter();
                //JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer
               // converter.ReadJson()
            }

        }

        public class WriteJsonMethod
        {

        }
    }
}