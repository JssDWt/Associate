namespace Societatis.HAL.Tests
{
    using System;
    using Newtonsoft.Json;
    using Societatis.HAL.Converters;
    using Xunit;

    public class LinkCollectionJsonReaderTests
    {
        public class LinkTypeProperty
        {
            [Fact]
            public void Default_TypeOfLink()
            {
                LinkCollectionJsonReader converter = new LinkCollectionJsonReader();
                Assert.Equal(typeof(Link), converter.LinkType);
            }

            [Fact]
            public void AbstractType_Throws()
            {
                var converter = new LinkCollectionJsonReader();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(AbstractLink));
            }

            [Fact]
            public void InterfaceType_Throws()
            {
                var converter = new LinkCollectionJsonReader();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(ILink));
            }

            [Fact]
            public void NoDefaultConstructor_Throws()
            {
                var converter = new LinkCollectionJsonReader();
                Assert.Throws<ArgumentException>(() => converter.LinkType = typeof(LinkWithoutDefaultConstructor));
            }

            [Fact]
            public void ConcreteType_IsFine()
            {
                var converter = new LinkCollectionJsonReader();
                converter.LinkType = typeof(ConcreteLink);
                Assert.Equal(typeof(ConcreteLink), converter.LinkType);
            }
        }

        public class CanConvertMethod
        {
            [Theory]
            [InlineData(typeof(RelationCollection<ILink>))]
            [InlineData(typeof(RelationCollection<Link>))]
            [InlineData(typeof(LinkCollection))]
            [InlineData(typeof(ConcreteLinkCollection))]
            public void CanConvert_IRelationCollectionTypes(Type type)
            {
                JsonConverter converter = new LinkCollectionJsonReader();
                bool actual = converter.CanConvert(type);
                Assert.True(actual);
            }

            [Theory]
            [InlineData(typeof(IRelationCollection<Link>))]
            [InlineData(typeof(IRelationCollection<ILink>))]
            [InlineData(typeof(RelationCollection<IResource>))]
            [InlineData(typeof(AbstractLinkCollection))]
            [InlineData(typeof(object))]
            public void CannotConvert_OtherTypes(Type type)
            {
                JsonConverter converter = new LinkCollectionJsonReader();
                bool actual = converter.CanConvert(type);
                Assert.False(actual);
            }
        }
    }
}