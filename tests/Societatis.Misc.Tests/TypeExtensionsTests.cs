namespace Societatis.Misc.Tests
{
    using System;
    using System.Collections;
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
            public void List_True(Type type)
            {
                //Given
                var list = new List<string>();
                //When
                bool result = list.IsInstanceOfGenericType(type);
                //Then
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(Dictionary<,>))]
            [InlineData(typeof(System.Collections.Concurrent.ConcurrentQueue<>))]
            public void OtherType_False(Type type)
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
            [InlineData(typeof(System.Xml.XmlReader))]
            [InlineData(typeof(string))]
            [InlineData(typeof(System.Collections.ICollection))]
            public void NotGeneric_False(Type type)
            {
                var list = new List<string>();
                bool result = list.IsInstanceOfGenericType(type);
                Assert.False(result);
            }
        }

        public class IsOfGenericTypeMethod
        {
            [Theory]
            [InlineData(typeof(List<>), typeof(IEnumerable<>))]
            [InlineData(typeof(List<>), typeof(ICollection<>))]
            [InlineData(typeof(List<string>), typeof(IEnumerable<>))]
            [InlineData(typeof(List<string>), typeof(IEnumerable<string>))]
            [InlineData(typeof(ICollection<>), typeof(IEnumerable<>))]
            public void ValidItems(Type toCompare, Type compareTo)
            {
                bool result = toCompare.IsOfGenericType(compareTo);
                Assert.True(result);
            }

            [Theory]
            [InlineData(typeof(List<>), null)]
            [InlineData(null, typeof(ICollection<>))]
            [InlineData(typeof(List<string>), typeof(IEnumerable<int>))]
            [InlineData(typeof(List<string>), typeof(IList))]
            [InlineData(typeof(IEnumerable<>), typeof(List<string>))]
            public void InvalidItems(Type toCompare, Type compareTo)
            {
                bool result = toCompare.IsOfGenericType(compareTo);
                Assert.False(result);
            }
        }
    }
}
