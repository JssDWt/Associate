namespace Societatis.HAL.Tests
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Societatis.HAL.Converters;
    using Xunit;

    public class ResourceJsonConverterTests
    {
        public virtual string TestDataPath => Path.Combine("TestData", nameof(ResourceJsonConverterTests));

        public class ReadJsonMethod : ResourceJsonConverterTests
        {
            public override string TestDataPath => Path.Combine(base.TestDataPath, nameof(ReadJsonMethod));

            [Fact]
            public void ReadTestResource()
            {
                object result = null;
                using (var file = File.OpenText(Path.Combine(this.TestDataPath, "HALSpecification.json")))
                using (var reader = new JsonTextReader(file))
                {
                    var serializer = new JsonSerializer();
                    var resourceCollectionReader = new ResourceCollectionJsonReader();
                    var mappingsConverter = new MappingJsonConverter();
                    mappingsConverter.Mappings[typeof(IRelationCollection<ILink>)] = typeof(LinkCollection);
                    mappingsConverter.Mappings[typeof(IRelationCollection<IResource>)] = typeof(RelationCollection<IResource>);
                    serializer.Converters.Add(resourceCollectionReader);
                    serializer.Converters.Add(new LinkCollectionJsonReader());
                    serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    JsonConverter converter = new ResourceJsonConverter();
                    result = converter.ReadJson(reader, typeof(HALSpecResource), null, serializer);
                }
            }
        }
    }
}