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
                Assert.Throws<ArgumentNullException>(() => collection.Add("other", null));
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

        public class AllProperty
        {
            [Fact]
            public void SeveralRels_ReturnsAllLinks()
            {
                var expected = new Dictionary<string, Link>
                {
                    {"first", new Link(new Uri("/orders", UriKind.Relative))},
                    {"first", new Link(new Uri("/details", UriKind.Relative))},
                    {"second", new Link(new Uri("/history", UriKind.Relative))},
                    {"third", new Link(new Uri("/obscureLink", UriKind.Relative))}
                };

                var links = new LinkCollection();
                foreach (var link in expected)
                {
                    links.Add(link.Key, link.Value);
                }

                Assert.False(expected.Values.Except(links.All).Any());
                Assert.False(links.All.Except(expected.Values).Any());
            }

            [Fact]
            public void NoLinks_ReturnsEmptyEnumerable()
            {
                var links = new LinkCollection();
                var result = links.All;
                Assert.NotNull(result);
                Assert.False(result.Any());
            }
        }

        public class ClearMethod
        {
            [Fact]
            public void MultipleAdded_ClearsAll()
            {
                var expected = new Dictionary<string, Link>
                {
                    {"first", new Link(new Uri("/orders", UriKind.Relative))},
                    {"first", new Link(new Uri("/details", UriKind.Relative))},
                    {"second", new Link(new Uri("/history", UriKind.Relative))},
                    {"third", new Link(new Uri("/obscureLink", UriKind.Relative))}
                };

                var links = new LinkCollection();
                foreach (var link in expected)
                {
                    links.Add(link.Key, link.Value);
                }

                links.Clear();

                Assert.False(links.All.Any());
            }

            [Fact]
            public void NoneExist_StillWorks()
            {
                var links = new LinkCollection();
                links.Clear();

                // Great, no exceptions.
            }
        }
    }
}