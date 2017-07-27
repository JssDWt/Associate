namespace Societatis.HAL.Tests
{
    using System;
    using Newtonsoft.Json;
    using Societatis.HAL.Converters;
    using Xunit;

    public class LinkCollectionJsonConverterTests
    {
        public class LinkTypeProperty
        {
            [Fact]
            public void Default_TypeOfLink()
            {
                LinkCollectionJsonConverter converter = new LinkCollectionJsonConverter();
                Assert.Equal(typeof(Link), converter.LinkType);
            }

            [Fact]
            public void AbstractType_Throws()
            {
                var converter = new LinkCollectionJsonConverter();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(AbstractLink));
            }

            [Fact]
            public void InterfaceType_Throws()
            {
                var converter = new LinkCollectionJsonConverter();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(ILink));
            }

            [Fact]
            public void NoDefaultConstructor_Throws()
            {
                var converter = new LinkCollectionJsonConverter();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(LinkWithoutDefaultConstructor));
            }

            [Fact]
            public void ConcreteType_IsFine()
            {
                var converter = new LinkCollectionJsonConverter();
                converter.LinkType = typeof(ConcreteLink);
                Assert.Equal(typeof(ConcreteLink), converter.LinkType);
            }
        }
    }
}