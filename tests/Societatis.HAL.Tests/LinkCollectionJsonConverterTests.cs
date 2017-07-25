namespace Societatis.HAL.Tests
{
    using System;
    using Newtonsoft.Json;
    using Societatis.HAL.Converters;
    using Xunit;

    public class LinkCollectionJsonConverterTests
    {
        public class Constructor
        {
            [Fact]
            public void DefaultConstructor_LinkTypeLink()
            {
                LinkCollectionJsonConverter converter = new LinkCollectionJsonConverter();
                Assert.Equal(typeof(Link), converter.LinkType);
            }
        }

        public class LinkTypeProperty
        {
            [Fact]
            public void AbstractType_Throws()
            {
                var converter = new LinkCollectionJsonConverter();
                Assert.Throws<ArgumentException>(() => (converter.LinkType = typeof(AbstractLink));
            }
        }
    }
}