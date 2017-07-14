namespace Societatis.HAL.Tests
{
    using System;
    using Xunit;

    public class ResourceTests
    {
        public class DefaultConstructor
        {
            [Fact]
            public void EmbeddedHasLinksAsDependancy()
            {
                var resource = new TestResource();
                Assert.NotNull(resource.Links);
                Assert.NotNull(resource.Embedded);
                Assert.Equal(resource.Links.SingleRelations, resource.Embedded.SingleRelations);

                resource.Links.SingleRelations.Add("second");

                // They should still be equal after updating the single links.
                Assert.Equal(resource.Links.SingleRelations, resource.Embedded.SingleRelations);
            }
        }

        public class LinksEmbeddedConstructor
        {
            [Fact]
            public void LinksAndEmbeddedAreSet()
            {
                var expectedLinks = new LinkCollection();
                expectedLinks.SingleRelations.Add("other");

                var expectedEmbedded = new ResourceCollection(expectedLinks);
                
                var resource = new TestResource(expectedLinks, expectedEmbedded);
                Assert.Equal(expectedLinks, resource.Links);
                Assert.Equal(expectedEmbedded, resource.Embedded);
            }
        }
    }

    public class GenericResourceTests
    {
        public class ValueConstructor
        {
            [Fact]
            public void ValueCannotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => new Resource<object>(null));
            }

            [Fact]
            public void ValueIsSet()
            {
                var expectedValue = new object();
                var resource = new Resource<object>(expectedValue);
                Assert.Equal(expectedValue, resource.Data);
            }
        }

        public class ValueLinksEmbeddedConstructor
        {
            [Fact]
            public void ValueLinksAndEmbeddedAreSet()
            {
                var expectedValue = new object();
                var expectedLinks = new LinkCollection();
                expectedLinks.SingleRelations.Add("other");

                var expectedEmbedded = new ResourceCollection(expectedLinks);
                
                var resource = new Resource<object>(expectedValue, expectedLinks, expectedEmbedded);
                Assert.Equal(expectedValue, resource.Data);
                Assert.Equal(expectedLinks, resource.Links);
                Assert.Equal(expectedEmbedded, resource.Embedded);
            }
        }

        public class ValueProperty
        {
            [Fact]
            public void ThrowsIfNull()
            {
                var resource = new Resource<object>(new object());
                Assert.Throws<ArgumentNullException>(() => resource.Data = null);
            }
        }
    }
}