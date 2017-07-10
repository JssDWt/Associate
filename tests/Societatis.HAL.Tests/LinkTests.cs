namespace Societatis.HAL.Tests
{
    using System;
    using Xunit;
    public class LinkTests
    {
        public class UriConstructor
        {
            [Fact]
            public void Constructor_SetsHrefProperty()
            {
                var expectedUri = new Uri("/orders", UriKind.Relative);
                var link = new Link(expectedUri);
                Assert.Equal(expectedUri, link.HRef);
            }
        }

        public class HrefProperty
        {
            [Fact]
            public void CannotSetNull()
            {
                var link = new Link(new Uri("/orders", UriKind.Relative));
                Assert.Throws<ArgumentNullException>(() => link.HRef = null);
            }
        }
    }
}