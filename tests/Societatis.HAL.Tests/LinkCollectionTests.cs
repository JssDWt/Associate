namespace Societatis.HAL.Tests
{
    using System;
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
        }
    }
}