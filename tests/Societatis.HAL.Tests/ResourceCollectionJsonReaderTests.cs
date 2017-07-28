namespace Societatis.HAL.Tests
{
    using System;
    using Newtonsoft.Json;
    using Societatis.HAL.Converters;
    using Xunit;
    
    public class ResourceCollectionJsonReaderTests
    {
        public class CanConvertMethod
        {
            [Theory]
            [InlineData(typeof(RelationCollection<IResource>))]
            [InlineData(typeof(RelationCollection<Resource>))]
            [InlineData(typeof(RelationCollection<Resource<object>>))]
            [InlineData(typeof(ConcreteResourceCollection))]
            public void CanConvert_IRelationCollectionTypes(Type type)
            {
                JsonConverter converter = new ResourceCollectionJsonReader();
                bool actual = converter.CanConvert(type);
                Assert.True(actual);
            }

            [Theory]
            [InlineData(typeof(IRelationCollection<Resource>))]
            [InlineData(typeof(IRelationCollection<IResource>))]
            [InlineData(typeof(RelationCollection<ILink>))]
            [InlineData(typeof(AbstractResourceCollection))]
            [InlineData(typeof(object))]
            public void CannotConvert_OtherTypes(Type type)
            {
                JsonConverter converter = new ResourceCollectionJsonReader();
                bool actual = converter.CanConvert(type);
                Assert.False(actual);
            }
        }
    }
}