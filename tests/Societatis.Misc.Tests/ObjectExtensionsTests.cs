namespace Societatis.Misc.Tests
{
    using System;
    using Xunit;

    public class ObjectExtensionsTests
    {
        public class ThrowIfNullMethod
        {
            [Fact]
            public void ObjectNotNull_DoesNotThrow()
            {
                var obj = new object();
                obj.ThrowIfNull(nameof(obj));
            }

            [Fact]
            public void ObjectNull_ThrowsArgumentNullException()
            {
                object obj = null;
                ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(nameof(obj)));
                Assert.Equal("obj", ex.ParamName);
            }
        }
    }
}
