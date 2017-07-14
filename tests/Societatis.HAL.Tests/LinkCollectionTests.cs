namespace Societatis.HAL.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class LinkCollectionTests
    {
        public class DefaultConstructor
        {
            [Fact]
            public void SelfIsSingleRelation()
            {
                var links = new LinkCollection();
                Assert.Equal(new string[] {"self"}, links.SingleRelations);
            }
        }

        public class AddMethod
        {
            [Fact]
            public void ThrowsIfLinkNull()
            {
                var collection = new LinkCollection();
                Assert.Throws<ArgumentNullException>(() => collection.Add("other", null as ILink));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void ThrowsIfRelNullOrWhitespace(string rel)
            {

                var link = new Link(new Uri("/orders", UriKind.Relative));
                var collection = new LinkCollection();
                Assert.Throws<ArgumentException>(() => collection.Add(rel, link));
            }

            [Fact]
            public void AddsToExistingRelation()
            {
                var firstLink = new Link(new Uri("/orders", UriKind.Relative));
                var secondLink = new Link(new Uri("/details", UriKind.Relative));
                var expectedLinks = new ILink[] { firstLink, secondLink };
                var links = new LinkCollection();
                links.Add("other", firstLink);
                links.Add("other", secondLink);

                Assert.Equal(expectedLinks, links["other"]);
                Assert.Equal(expectedLinks, links.Get("other"));
            }

            [Fact]
            public void AddTwoToSingleRelation_Throws()
            {
                var firstLink = new Link(new Uri("/orders", UriKind.Relative));
                var secondLink = new Link(new Uri("/details", UriKind.Relative));

                var links = new LinkCollection();
                links.SingleRelations.Add("other");
                links.Add("other", firstLink);
                Assert.Throws<InvalidOperationException>(() => links.Add("other", secondLink));
            }

            [Fact]
            public void AddTwoAtOnce_BothAdded()
            {
                var expectedLinks = new ILink[] 
                { 
                    new Link(new Uri("/orders", UriKind.Relative)), 
                    new Link(new Uri("/details", UriKind.Relative)) 
                };
                var links = new LinkCollection();
                links.Add("other", expectedLinks);

                Assert.Equal(expectedLinks, links["other"]);
            }

            [Fact]
            public void AddInvalidCurie_Throws()
            {
                var curie = new Link(new Uri("/orders", UriKind.Relative));
                var links = new LinkCollection();
                Assert.Throws<InvalidOperationException>(() => links.Add("curies", curie));
            }

            [Fact]
            public void AddValidCurie_IsAdded()
            {
                var curie = new Link(new Uri("/orders/{rel}", UriKind.Relative)) {Name="myCurie", Templated = true};
                var links = new LinkCollection();
                links.Add("curies", curie);
                Assert.Equal(1, links.Curies.Count());
                Assert.Equal(curie, links.Curies.Single());
            }
        }

        public class CuriesProperty
        {
            [Fact]
            public void Default_EmptyEnumerable()
            {
                var links = new LinkCollection();
                Assert.NotNull(links.Curies);
                Assert.False(links.Curies.Any());
            }

            [Fact]
            public void ReturnsAllCuries()
            {
                var links = new LinkCollection();
                var expected = new Curie[]
                {
                    new Curie("first", new Uri("/orders/{rel}", UriKind.Relative)),
                    new Curie("second", new Uri("/details/{rel}", UriKind.Relative)),
                    new Curie("third", new Uri("/magnificent/{rel}", UriKind.Relative)),
                };

                foreach (var curie in expected)
                {
                    links.Add("curies", curie);
                }

                Assert.Equal(expected, links.Curies);
            }
        }

        public class IsSingleRelationMethod
        {
            [Fact]
            public void Default_SelfReturnsTrue()
            {
                var links = new LinkCollection();
                bool result = links.IsSingleRelation("self");
                Assert.True(result);
            }
        }

        public class SelfProperty
        {
            [Fact]
            public void Default_Null()
            {
                var links = new LinkCollection();
                ILink result = links.Self;
                Assert.Null(result);
            }

            [Fact]
            public void ContainsSelfLink()
            {
                var links = new LinkCollection();
                var expected = new Link(new Uri("/self", UriKind.Relative));
                links.Add("self", expected);
                ILink actual = links.Self;
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsFirstIfMoreThanOne()
            {
                // Arrange
                var links = new LinkCollection();
                links.SingleRelations.Remove("self");
                var first = new Link(new Uri("/first", UriKind.Relative));
                var second = new Link(new Uri("/second", UriKind.Relative));
                links.Add("self", first);
                links.Add("self", second);

                // Act
                ILink actual = links.Self;

                // Assert
                Assert.Equal(first, actual);
            }
        }
    }
}