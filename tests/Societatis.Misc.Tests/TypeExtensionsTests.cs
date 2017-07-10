namespace Societatis.Misc.Tests
{
    using System;
    using System.Collections.Generic;
    using Societatis.Misc;
    using Xunit;

    public class TypeExtensionsTests
    {
        public class IsInstanceOfGenericTypeMethod
        {
            [Theory]
            [InlineData(typeof(List<>))]
            [InlineData(typeof(ICollection<>))]
            [InlineData(typeof(IEnumerable<>))]
            public void IsInstanceOfGenericType_List(Type type)
            {
                //Given
                var list = new List<string>();
                //When
                bool result = list.IsInstanceOfGenericType(type);
                //Then
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(Dictionary<>))]
            [InlineData(typeof(GenericList<>))]
            public void IsInstanceOfGenericType_OtherType(Type type)
            {
                //Given
                var list = new List<string>();
                //When
                bool result = list.IsInstanceOfGenericType(type);
                //Then
                Assert.False(result);
            }

            [Theory]
            [InlineData(typeof(object))]
            [InlineData(typeof(Collection))]
            [InlineData(typeof(string))]
            [InlineData(typeof(object))]
            public void IsInstanceOfGenericType_ThrowsIfNotGeneric()
            {
                var list = new List<string>();
                var ex = Assert.Throws<ArgumentException>(() => list.IsInstanceOfGenericType(typeof(object)));
            }
        }
    }
}