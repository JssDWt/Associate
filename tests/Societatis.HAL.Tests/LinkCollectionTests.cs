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

        public class ContainsMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void Rel_NullOrWhitespaceRel_Throws(string rel)
            {
                var links= new LinkCollection();
                Assert.Throws<ArgumentException>(() => links.Contains(rel));
            }

            [Fact]
            public void Rel_ExistingRel_ReturnsTrue()
            {
                var links = new LinkCollection();
                links.Add("other", new Link(new Uri("/orders", UriKind.Relative)));
                bool result = links.Contains("other");
                Assert.True(result);
            }

            [Fact]
            public void Rel_NonExistingRel_ReturnsFalse()
            {
                var links = new LinkCollection();
                links.Add("first", new Link(new Uri("/orders", UriKind.Relative)));
                bool result = links.Contains("second");
                Assert.False(result);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void RelLink_NullOrWhitespaceRel_Throws(string rel)
            {
                var links= new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                Assert.Throws<ArgumentException>(() => links.Contains(rel, link));
            }

            [Fact]
            public void RelLink_NullLink_Throws()
            {
                var links = new LinkCollection();
                links.Add("other", new Link(new Uri("/orders", UriKind.Relative)));
                Assert.Throws<ArgumentNullException>(() => links.Contains("other", null));
            }

            [Fact]
            public void RelLink_ExistingLink_ReturnsTrue()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                links.Add("other", link);
                bool result = links.Contains("other", link);
                Assert.True(result);
            }

            [Fact]
            public void RelLink_NonExistingLink_ReturnsFalse()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                var otherLink = new Link(new Uri("/details", UriKind.Relative));
                links.Add("other", link);
                bool result = links.Contains("other", otherLink);
                Assert.False(result);
            }

            [Fact]
            public void RelLink_NonExistingRel_ReturnsFalse()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                links.Add("first", link);
                bool result = links.Contains("second", link);
                Assert.False(result);
            }

            [Fact]
            public void Link_NullLink_Throws()
            {
                var links = new LinkCollection();
                links.Add("other", new Link(new Uri("/orders", UriKind.Relative)));
                Assert.Throws<ArgumentNullException>(() => links.Contains(null as ILink));
            }

            [Fact]
            public void Link_ExistingLink_ReturnsTrue()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                links.Add("other", link);
                bool result = links.Contains(link);
                Assert.True(result);
            }

            [Fact]
            public void Link_NonExistingLink_ReturnsFalse()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                var otherLink = new Link(new Uri("/details", UriKind.Relative));
                links.Add("other", link);
                bool result = links.Contains(otherLink);
                Assert.False(result);
            }
        }

        public class CountProperty
        {
            [Fact]
            public void CountsAllLinks()
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

                Assert.Equal(expected.Count, links.Count);
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

        public class GetMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void NullOrWhitespaceRel_Throws(string rel)
            {
                var links = new LinkCollection();
                Assert.Throws<ArgumentException>(() => links.Get(rel));
            }

            [Fact]
            public void NotFound_ReturnsNull()
            {
                var links = new LinkCollection();
                var result = links.Get("other");
                Assert.Null(result);
            }

            [Fact]
            public void AddedAndRemoved_ReturnsNull()
            {
                var links = new LinkCollection();
                var link = new Link(new Uri("/orders", UriKind.Relative));
                links.Add("other", link);
                links.Remove("other");
                var result = links.Get("other");
                Assert.Null(result);
            }

            [Fact]
            public void ReturnsAllLinks()
            {
                var expected = new Link[]
                {
                    new Link(new Uri("/orders", UriKind.Relative)),
                    new Link(new Uri("/details", UriKind.Relative)),
                    new Link(new Uri("/person", UriKind.Relative))
                };

                var links = new LinkCollection();
                foreach(var link in expected)
                {
                    links.Add("other", link);
                }

                links.Add("first", new Link(new Uri("/somelink", UriKind.Relative)));

                var actual = links.Get("other");

                Assert.Equal(expected, actual);
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

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void RelNullOrWhitespace_Throws(string rel)
            {
                var links = new LinkCollection();
                Assert.Throws<ArgumentException>(() => links.IsSingleRelation(rel));
            }

            [Fact]
            public void AddedSingleRelation_ReturnsTrue()
            {
                var links = new LinkCollection();
                links.SingleRelations.Add("other");
                bool result = links.IsSingleRelation("other");
                Assert.True(result);
            }

            [Fact]
            public void NotAddedSingleRelation_ReturnsFalse()
            {
                var links = new LinkCollection();
                links.SingleRelations.Add("first");
                bool result = links.IsSingleRelation("second");
                Assert.False(result);
            }
        }

        public class RelationCountProperty
        {
            [Fact]
            public void Default_Zero()
            {
                var links = new LinkCollection();
                var result = links.RelationCount;
                Assert.Equal(0, result);
            }

            [Fact]
            public void CountAllRelations()
            {
                var link = new Link(new Uri("/orders", UriKind.Relative));
                var otherLink = new Link(new Uri("/other", UriKind.Relative));
                var links = new LinkCollection();
                links.Add("first", link);
                links.Add("second", link);
                links.Add("second", otherLink);
                links.Add("third", link);
                links.Add("fourth", link);

                Assert.Equal(4, links.RelationCount);
            }
        }

        public class RelationsProperty
        {
            [Fact]
            public void Default_Empty()
            {
                var links = new LinkCollection();
                var result = links.Relations;
                Assert.NotNull(result);
                Assert.False(result.Any());
            }

            [Fact]
            public void ReturnsAllRelations()
            {
                string[] expected = new string[]
                {
                   "first" ,
                   "second",
                   "third",
                   "fourth"
                };

                var link = new Link(new Uri("/orders", UriKind.Relative));
                var otherLink = new Link(new Uri("/other", UriKind.Relative));
                var links = new LinkCollection();
                links.Add("first", link);
                links.Add("second", link);
                links.Add("second", otherLink);
                links.Add("third", link);
                links.Add("fourth", link);

                Assert.Equal(expected, links.Relations);
            }
        }

        public class RemoveMethod
        {

        }
    }
}