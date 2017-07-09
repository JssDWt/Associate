namespace Societatis.Misc.Tests
{
    using System;
    using Xunit;

    public class StringExtensionsTests
    {
        public class ThrowIfNullOrWhiteSpaceMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\r\n")]
            [InlineData("\r")]
            [InlineData("\n")]
            [InlineData("\t")]
            [InlineData(" \t\r\n")]
            public void ThrowsIfNullOrWhiteSpace(string str)
            {
                ArgumentException ex = Assert.Throws<ArgumentException>(() => str.ThrowIfNullOrWhiteSpace(nameof(str)));
            }

            [Fact]
            public void NameMatchesParamName()
            {
                string myName = "";
                string expectedName = "myName";
                ArgumentException ex = Assert.Throws<ArgumentException>(() => myName.ThrowIfNullOrWhiteSpace(nameof(myName)));
                Assert.Equal(expectedName, ex.ParamName);
            }

            [Fact]
            public void CustomMessageMatches()
            {
                string myName = "";
                string inputMessage = "My custom message";
                string expectedMessage = inputMessage + "\r\nParameter name: myName";
                ArgumentException ex = Assert.Throws<ArgumentException>(() => myName.ThrowIfNullOrWhiteSpace(nameof(myName), inputMessage));
                Assert.Equal(expectedMessage, ex.Message);
            }
        }
    }
}