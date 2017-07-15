namespace Societatis.HAL.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Societatis.HAL;
    using Xunit;

    public class RelationCollectionJsonConverterTests
    {
        public virtual string TestDataPath => Path.Combine("TestData", nameof(RelationCollectionJsonConverterTests));

        public class CanConvertMethod
        {
            [Theory]
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

        public class ReadJsonMethod : RelationCollectionJsonConverterTests
        {
            public override string TestDataPath => Path.Combine(base.TestDataPath, nameof(ReadJsonMethod));

            [Fact]
            public void LinkCollection_Simple()
            {
                object result = null;
                JsonConverter converter = new RelationCollectionJsonConverter();
                using (var streamReader = File.OpenText(Path.Combine(this.TestDataPath, "LinkCollection_Simple.json")))
                using (var reader = new JsonTextReader(streamReader))
                {
                    result = converter.ReadJson(reader, typeof(RelationCollection<Link>), null, new JsonSerializer());
                }

                var typedResult = Assert.IsType<LinkCollection>(result);
                Assert.Equal(3, typedResult.Count);
                Assert.Equal(3, typedResult.ItemCount);
                Assert.Equal(new string[] { "self", "next", "find" }, typedResult.Relations);
                Assert.Equal(1, typedResult["self"].Count());
                Assert.Equal(new Uri("/orders"), typedResult["self"].Single().HRef);
            }

        }

        public class WriteJsonMethod: RelationCollectionJsonConverterTests
        {
            public override string TestDataPath => Path.Combine(base.TestDataPath, nameof(WriteJsonMethod));

            [Fact]
            public void LinkCollection_Simple()
            {
                JToken expectedJson = this.GetJsonFromFile("LinkCollection_Simple.json");

                IRelationCollection<ILink> collection = new LinkCollection();
                collection.SingleRelations.Add("next");
                collection.SingleRelations.Add("find");
                collection.Set("self", new Link(new Uri("/orders", UriKind.Relative)));
                collection.Set("next", new Link(new Uri("/orders?page=2", UriKind.Relative)));
                collection.Set("find", new Link(new Uri("/orders{?id}", UriKind.Relative)) {Templated = true});

                JToken actualJson = null;
                using (var actualStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(actualStream, Encoding.UTF8, 4096, true))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonConverter converter = new RelationCollectionJsonConverter();
                        converter.WriteJson(jsonWriter, collection, new JsonSerializer());
                    }

                    actualJson = this.GetJsonFromStream(actualStream);
                }
                
                Assert.True(JToken.DeepEquals(expectedJson, actualJson));
            }

            [Fact]
            public void LinkCollection_WithCuries()
            {
                JToken expectedJson = this.GetJsonFromFile("LinkCollection_WithCuries.json");

                var links = new LinkCollection();
                links.Set("self", new Link(new Uri("/orders", UriKind.Relative)));
                links.Add("curies", new Curie("myCurie", new Uri("/orders/{rel}", UriKind.Relative)));
                links.Add("myCurie:detail", new Link(new Uri("/detail", UriKind.Relative)) {Templated = true});

                JToken actualJson = null;
                using (var actualStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(actualStream, Encoding.UTF8, 4096, true))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonConverter converter = new RelationCollectionJsonConverter();
                        converter.WriteJson(jsonWriter, links, new JsonSerializer());
                    }

                    actualJson = this.GetJsonFromStream(actualStream);
                }

                Assert.True(JToken.DeepEquals(expectedJson, actualJson));
            }

            [Fact]
            public void TestRelationCollection()
            {
                JToken expectedJson = this.GetJsonFromFile("TestRelationCollection.json");

                var testRelations = new TestRelationCollection();
                testRelations.Set("self", new TestClass() {TestProperty = "TestValue1"});
                testRelations.Set("other", new TestClass() {TestProperty = "TestValue2"});

                JToken actualJson = null;
                using (var actualStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(actualStream, Encoding.UTF8, 4096, true))
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        JsonConverter converter = new RelationCollectionJsonConverter();
                        converter.WriteJson(jsonWriter, testRelations, new JsonSerializer());
                    }

                    actualJson = this.GetJsonFromStream(actualStream);
                }

                Assert.True(JToken.DeepEquals(expectedJson, actualJson));
            }

            private JToken GetJsonFromFile(string fileName)
            {
                JToken result = null;
                using (var jsonFileStream = File.OpenRead(Path.Combine(this.TestDataPath, fileName)))
                {
                    result = this.GetJsonFromStream(jsonFileStream);
                }

                return result;
            }

            private JToken GetJsonFromStream(Stream stream)
            {
                JToken result;

                stream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(stream, Encoding.UTF8, false, 4096, true))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    result = JToken.ReadFrom(jsonReader);
                }

                return result;
            }
        }
    }
}