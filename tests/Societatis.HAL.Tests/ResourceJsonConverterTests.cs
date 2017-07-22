namespace Societatis.HAL.Tests
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
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
                    serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    JsonConverter converter = new ResourceJsonConverter();
                    result = converter.ReadJson(reader, typeof(HALSpecResource), null, serializer);
                }
            }
        }
    }
}