namespace Societatis.HAL.Tests
{
    using System;
    using Xunit;

    public class ResourceCollectionTests
    {
        public class Constructor
        {
            [Fact]
            public void Links_ActiveReferenceToSingleRelations()
            {
                var links = new RelationCollection<string>();
                links.SingleRelations.Add("first");
                links.SingleRelations.Add("second");

                var resources = new ResourceCollection(links);
                Assert.Equal(links.SingleRelations, resources.SingleRelations);

                links.SingleRelations.Add("third");
                Assert.Equal(links.SingleRelations, resources.SingleRelations);

                resources.SingleRelations.Add("fourth");
                Assert.Equal(links.SingleRelations, resources.SingleRelations);
            }
        }

        public class AddMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void RelNullOrWhitespace_Throws(string rel)
            {
                var resources = new ResourceCollection(new RelationCollection<string>());
                Assert.Throws<ArgumentException>(() => resources.Add(rel, new TestResource()));
            }

            [Fact]
            public void ResourceNull_Throws()
            {
                var resources = new ResourceCollection(new RelationCollection<string>());
                Assert.Throws<ArgumentNullException>(() => resources.Add("rel", null as IResource));
            }

            [Fact]
            public void NotAllowedRelation_Throws()
            {
                var links = new RelationCollection<string>();
                links.Add("first", "item");
                links.Add("second", "thing");
                var resources = new ResourceCollection(links);

                Assert.Throws<InvalidOperationException>(() => resources.Add("third", new TestResource()));
            }

            [Fact]
            public void Multiple_NotAllowedRelation_Throws()
            {
                var links = new RelationCollection<string>();
                links.Add("first", "item");
                links.Add("second", "thing");
                var resources = new ResourceCollection(links);
                var toAdd = new IResource[] { new TestResource(), new TestResource() };
                Assert.Throws<InvalidOperationException>(() => resources.Add("third", toAdd));
            }

            [Fact]
            public void AllowedRelation_IsAdded()
            {
                var links = new RelationCollection<string>();
                links.Add("first", "item");
                var resources = new ResourceCollection(links);

                var resource = new TestResource();
                resources.Add("first", resource);
                Assert.True(resources.Contains("first", resource));
            }
        }
    }
}