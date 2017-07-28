namespace Societatis.HAL.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.HAL;
    using Societatis.HAL.Converters;
    using Xunit;

    public class RelationCollectionJsonReaderTests
    {
        public virtual string TestDataPath => Path.Combine("TestData", nameof(RelationCollectionJsonReaderTests));

        public class CanConvertMethod
        {
            [Theory]
            [InlineData(typeof(RelationCollection<int>))]
            [InlineData(typeof(RelationCollection<Link>))]
            [InlineData(typeof(RelationCollection<ILink>))]
            [InlineData(typeof(RelationCollection<Resource>))]
            [InlineData(typeof(RelationCollection<Resource<object>>))]
            [InlineData(typeof(LinkCollection))]
            [InlineData(typeof(TestRelationCollection))]
            [InlineData(typeof(HighlyInheritedRelationCollection))]
            [InlineData(typeof(ConcreteLinkCollection))]
            [InlineData(typeof(ConcreteResourceCollection))]
            public void ConcreteTypes_ReturnTrue(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonReader<object>();
                bool result = converter.CanConvert(type);
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(object))]
            [InlineData(typeof(System.Collections.Generic.List<string>))]
            [InlineData(typeof(IRelationCollection<>))]
            [InlineData(typeof(RelationCollection<>))]
            [InlineData(typeof(IRelationCollection<object>))]
            public void OtherTypes_ReturnFalse(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonReader<object>();
                bool result = converter.CanConvert(type);
                Assert.False(result);
            }

            [Theory]
            [InlineData(typeof(RelationCollection<List<string>>))]
            [InlineData(typeof(RelationCollection<ICollection<string>>))]
            [InlineData(typeof(RelationCollection<HashSet<string>>))]
            public void SpecificType_ReturnsTrue(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonReader<ICollection<string>>();
                bool result = converter.CanConvert(type);
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(IRelationCollection<List<string>>))]
            [InlineData(typeof(RelationCollection<ICollection<int>>))]
            [InlineData(typeof(RelationCollection<object>))]
            public void SpecificOtherType_ReturnsFalse(Type type)
            {
                JsonConverter converter = new RelationCollectionJsonReader<ICollection<string>>();
                bool result = converter.CanConvert(type);
                Assert.False(result);
            }
        }

        public class ReadJsonMethod : RelationCollectionJsonReaderTests
        {
            public override string TestDataPath => Path.Combine(base.TestDataPath, nameof(ReadJsonMethod));

            [Fact]
            public void LinkCollection_Simple()
            {
                object result = null;
                JsonConverter converter = new RelationCollectionJsonReader<Link>();
                using (var streamReader = File.OpenText(Path.Combine(this.TestDataPath, "LinkCollection_Simple.json")))
                using (var reader = new JsonTextReader(streamReader))
                {
                    result = converter.ReadJson(reader, typeof(RelationCollection<Link>), null, new JsonSerializer());
                }

                var typedResult = Assert.IsType<RelationCollection<Link>>(result);
                Assert.Equal(3, typedResult.Count);
                Assert.Equal(new string[] { "self", "next", "find" }, typedResult.RelationNames);
                Assert.Equal(new string[] { "self", "next", "find" }, typedResult.SingularRelations);
                Assert.Equal(1, typedResult["self"].Count);
                Assert.Equal(new Uri("/orders", UriKind.Relative), typedResult["self"].Single().HRef);
            }
        }
    }
}