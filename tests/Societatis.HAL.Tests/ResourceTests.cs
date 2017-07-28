namespace Societatis.HAL.Tests
{
    using System;
    using Xunit;

    public class ResourceTests
    {
        public class LinksEmbeddedConstructor
        {
            [Fact]
            public void LinksAndEmbeddedAreSet()
            {
                var expectedLinks = new LinkCollection();
                expectedLinks.MarkSingular("other");

                var expectedEmbedded = new RelationCollection<IResource>();
                
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
                expectedLinks.MarkSingular("other");

                var expectedEmbedded = new RelationCollection<IResource>();
                
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

            [Fact]
            public void ThrowsIfIResource()
            {
                Assert.Throws<ArgumentException>(() => new Resource<TestResource>(new TestResource()));
            }
        }
    }
}