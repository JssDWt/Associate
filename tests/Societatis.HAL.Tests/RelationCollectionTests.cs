namespace Societatis.HAL.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class RelationCollectionTests
    {
        public class AddMethod
        {
            [Fact]
            public void ThrowsIfItemNull()
            {
                var relations = new RelationCollection<string>();
                Assert.Throws<ArgumentNullException>(() => relations.Add("other", null as string));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void ThrowsIfRelNullOrWhitespace(string rel)
            {
                var relations = new RelationCollection<string>();
                Assert.Throws<ArgumentException>(() => relations.Add(rel, "something"));
            }

            [Fact]
            public void AddsToExistingRelation()
            {
                string firstItem = "something";
                string secondItem = "else";
                var expectedItems = new string[] { firstItem, secondItem };
                var relations = new RelationCollection<string>();
                relations.Add("other", firstItem);
                relations.Add("other", secondItem);

                Assert.Equal(expectedItems, relations["other"]);
                Assert.Equal(expectedItems, relations.Get("other"));
            }

            [Fact]
            public void AddTwoToSingleRelation_Throws()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("other");
                relations.Add("other", "something");
                Assert.Throws<InvalidOperationException>(() => relations.Add("other", "else"));
            }

            [Fact]
            public void AddTwoAtOnce_BothAdded()
            {
                var expectedItems = new string[] 
                { 
                    "something",
                    "else"
                };
                var relations = new RelationCollection<string>();
                relations.Add("other", expectedItems);

                Assert.Equal(expectedItems, relations["other"]);
            }
        }

        public class AllProperty
        {
            [Fact]
            public void SeveralRels_ReturnsAllItems()
            {
                var expected =new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("first", "something"),
                    new Tuple<string, string>("first", "else"),
                    new Tuple<string, string>("second", "completely"),
                    new Tuple<string, string>("third", "different"),
                };

                var relations = new RelationCollection<string>();
                foreach (var link in expected)
                {
                    relations.Add(link.Item1, link.Item2);
                }

                Assert.False(expected.Select(e => e.Item2).Except(relations.All).Any());
                Assert.False(relations.All.Except(expected.Select(e => e.Item2)).Any());
            }

            [Fact]
            public void NoItems_ReturnsEmptyEnumerable()
            {
                var relations = new RelationCollection<string>();
                var result = relations.All;
                Assert.NotNull(result);
                Assert.False(result.Any());
            }
        }

        public class ClearMethod
        {
            [Fact]
            public void MultipleAdded_ClearsAll()
            {
                var expected = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("first", "something"),
                    new Tuple<string, string>("first", "else"),
                    new Tuple<string, string>("second", "completely"),
                    new Tuple<string, string>("third", "different"),
                };
                
                var relations = new RelationCollection<string>();
                foreach (var item in expected)
                {
                    relations.Add(item.Item1, item.Item2);
                }

                relations.Clear();

                Assert.Equal(0, relations.ItemCount);
            }

            [Fact]
            public void NoneExist_StillWorks()
            {
                var relations = new RelationCollection<string>();
                relations.Clear();

                // Great, no exceptions.
            }
        }

        public class ContainsMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void Rel_NullOrWhitespaceRel_ReturnsFalse(string rel)
            {
                var relations = new RelationCollection<string>();
                bool result = relations.Contains(rel);
                Assert.False(result);
            }

            [Fact]
            public void Rel_ExistingRel_ReturnsTrue()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                bool result = relations.Contains("other");
                Assert.True(result);
            }

            [Fact]
            public void Rel_NonExistingRel_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                relations.Add("first", "something");
                bool result = relations.Contains("second");
                Assert.False(result);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void RelItem_NullOrWhitespaceRel_ReturnsFalse(string rel)
            {
                var relations= new RelationCollection<string>();
                bool result = relations.Contains(rel, "seomthing");
                Assert.False(result);
            }

            [Fact]
            public void RelItem_NullLink_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                bool result = relations.Contains("other", null);
                Assert.False(result);
            }

            [Fact]
            public void RelItem_ExistingItem_ReturnsTrue()
            {
                var relations = new RelationCollection<string>();
                string item = "something";
                relations.Add("other", item);
                bool result = relations.Contains("other", item);
                Assert.True(result);
            }

            [Fact]
            public void RelItem_NonExistingItem_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                string item = "something";
                string otherItem = "else";
                relations.Add("other", item);
                bool result = relations.Contains("other", otherItem);
                Assert.False(result);
            }

            [Fact]
            public void RelItem_NonExistingRel_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                relations.Add("first", item);
                bool result = relations.Contains("second", item);
                Assert.False(result);
            }

            [Fact]
            public void Item_NullLink_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                bool result = relations.Contains(null as string);
                Assert.False(result);
            }

            [Fact]
            public void Item_ExistingItem_ReturnsTrue()
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                relations.Add("other", item);
                bool result = relations.Contains(item: item);
                Assert.True(result);
            }

            [Fact]
            public void Item_NonExistantItem_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                var otherItem = "else";
                relations.Add("other", item);
                bool result = relations.Contains(otherItem);
                Assert.False(result);
            }
        }

        public class CountProperty
        {
            [Fact]
            public void DefaultZero()
            {
                var relations = new RelationCollection<string>();
                Assert.Equal(0, relations.ItemCount);
            }

            [Fact]
            public void CountsAllItems()
            {
                var expected = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("first", "something"),
                    new Tuple<string, string>("first", "else"),
                    new Tuple<string, string>("second", "completely"),
                    new Tuple<string, string>("third", "different"),
                };
                
                var relations = new RelationCollection<string>();
                foreach (var item in expected)
                {
                    relations.Add(item.Item1, item.Item2);
                }

                Assert.Equal(expected.Count, relations.ItemCount);
            }
        }

        public class GetMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void NullOrWhitespaceRel_ReturnsEmpty(string rel)
            {
                var relations = new RelationCollection<string>();
                IEnumerable<string> result = relations.Get(rel);
                Assert.NotNull(result);
                Assert.False(result.Any());
            }

            [Fact]
            public void NotFound_ReturnsEmpty()
            {
                var relations = new RelationCollection<string>();
                var result = relations.Get("other");
                Assert.NotNull(result);
                Assert.False(result.Any());
            }

            [Fact]
            public void AddedAndRemoved_ReturnsEmpty()
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                relations.Add("other", item);
                relations.Remove("other");
                var result = relations.Get("other");
                Assert.NotNull(result);
                Assert.False(result.Any());
            }

            [Fact]
            public void ReturnsAllItems()
            {
                var expected = new string[]
                {
                    "something",
                    "super",
                    "great"
                };

                var relations = new RelationCollection<string>();
                foreach(var item in expected)
                {
                    relations.Add("other", item);
                }

                relations.Add("first", "outsider");

                var actual = relations.Get("other");

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void CannotCastBackToCollection()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "item");
                IEnumerable<string> items = relations.Get("other");
                Assert.NotNull(items);
                ICollection<string> result = null;
                Assert.Throws<InvalidCastException>(() => result = (ICollection<string>)items);
            }
        }

        public class IsSingleRelationMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void RelNullOrWhitespace_ReturnsFalse(string rel)
            {
                var relations = new RelationCollection<string>();
                bool result = relations.IsSingleRelation(rel);
                Assert.False(result);
            }

            [Fact]
            public void AddedSingleRelation_ReturnsTrue()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("other");
                bool result = relations.IsSingleRelation("other");
                Assert.True(result);
            }

            [Fact]
            public void NotAddedSingleRelation_ReturnsFalse()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("first");
                bool result = relations.IsSingleRelation("second");
                Assert.False(result);
            }
        }

        public class RelationCountProperty
        {
            [Fact]
            public void Default_Zero()
            {
                var relations = new RelationCollection<string>();
                var result = relations.Count;
                Assert.Equal(0, result);
            }

            [Fact]
            public void CountAllRelations()
            {
                var item = "something";
                var otherItem = "else";
                var relations = new RelationCollection<string>();
                relations.Add("first", item);
                relations.Add("second", item);
                relations.Add("second", otherItem);
                relations.Add("third", item);
                relations.Add("fourth", item);

                Assert.Equal(4, relations.Count);
            }
        }

        public class RelationsProperty
        {
            [Fact]
            public void Default_Empty()
            {
                var relations = new RelationCollection<string>();
                var result = relations.RelationNames;
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

                var item = "something";
                var otherItem = "different";
                var links = new RelationCollection<string>();
                links.Add("first", item);
                links.Add("second", item);
                links.Add("second", otherItem);
                links.Add("third", item);
                links.Add("fourth", item);

                Assert.Equal(expected, links.RelationNames);
            }
        }

        public class RemoveMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void NullOrWhitespaceRel_ReturnsFalse(string rel)
            {
                var relations = new RelationCollection<string>();
                bool result = relations.Remove(rel);
                Assert.False(result);
            }

            [Fact]
            public void NonExistantRel_ReturnsFalse()
            {
                var relations= new RelationCollection<string>();
                bool result = relations.Remove("other");
                Assert.False(result);
            }

            [Fact]
            public void ExistantRel_ReturnsTrue()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                bool result = relations.Remove("other");
                Assert.True(result);
            }

            [Fact]
            public void ExistantRel_IsRemoved()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                relations.Remove("other");
                Assert.Equal(0, relations.ItemCount);
            }
        }

        public class SetMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void ThrowsIfRelNullOrWhitespace(string rel)
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                Assert.Throws<ArgumentException>(() => relations.Set(rel, item));
            }

            [Fact]
            public void ThrowsIfItemNull()
            {
                var relations = new RelationCollection<string>();
                Assert.Throws<ArgumentNullException>(() => relations.Set("other", null as string));
            }

            [Fact]
            public void ThrowsIfItemsNull()
            {
                var relations = new RelationCollection<string>();
                Assert.Throws<ArgumentNullException>(() => relations.Set("other", null as IEnumerable<string>));
            }

            [Fact]
            public void DeletesIfItemsEmpty()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                
                relations.Set("other", Enumerable.Empty<string>());
                var result = relations.Get("other");
                Assert.NotNull(result);
                Assert.False(result.Any());
            }

            [Fact]
            public void AddsNewItem()
            {
                var relations = new RelationCollection<string>();
                var item = "item";
                relations.Set("other", item);
                bool result = relations.Contains("other", item);
                Assert.True(result);
            }

            [Fact]
            public void AddsMultipleItems()
            {
                var relations = new RelationCollection<string>();
                var expected = new string[] 
                {
                    "first",
                    "second",
                    "third"
                };
                relations.Set("other", expected);
                var actual = relations.Get("other");
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReplacesExistingItem()
            {
                var relations = new RelationCollection<string>();
                string expectedItem = "expected";
                string[] expectedItems = new string[] { expectedItem };
                relations.Add("other", "something");
                relations.Set("other", expectedItem);
                var actual = relations.Get("other");
                Assert.Equal(expectedItems, actual);
            }
        }

        public class SingleRelationsProperty
        {
            [Fact]
            public void Default_Empty()
            {
                var relations = new RelationCollection<string>();
                var result = relations.SingleRelations;
                Assert.NotNull(result);
                Assert.Equal(0, result.Count);
            }
            
            [Fact]
            public void CanAdd()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("other");
                Assert.True(relations.SingleRelations.Contains("other"));
            }

            [Fact]
            public void CanRemove()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("first");
                relations.SingleRelations.Add("second");

                relations.SingleRelations.Remove("first");
                Assert.False(relations.SingleRelations.Contains("first"));
                Assert.True(relations.SingleRelations.Contains("second"));
            }

            [Fact]
            public void DoubleAdd_AddedOnce()
            {
                var relations = new RelationCollection<string>();
                relations.SingleRelations.Add("first");
                relations.SingleRelations.Add("first");

                Assert.Equal(1, relations.SingleRelations.Count);
                Assert.True(relations.SingleRelations.Contains("first"));
            }
        }
    }
}