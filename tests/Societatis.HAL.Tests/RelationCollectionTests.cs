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
            public void DoesNotThrowIfItemNull()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", null as string);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void ThrowsIfRelNullOrWhitespace(string rel)
            {
                var relations = new RelationCollection<string>();
                Assert.ThrowsAny<ArgumentException>(() => relations.Add(rel, "something"));
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
                relations.MarkSingular("other");
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

                Assert.Equal(0, relations.Count);
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
        }

        public class CountProperty
        {
            [Fact]
            public void DefaultZero()
            {
                var relations = new RelationCollection<string>();
                Assert.Equal(0, relations.Count);
            }

            [Fact]
            public void CountsAllRelations()
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

                Assert.Equal(3, relations.Count);
            }
        }

        public class GetMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            public void NullOrWhitespaceRel_ReturnsNull(string rel)
            {
                var relations = new RelationCollection<string>();
                ICollection<string> result = relations.Get(rel);
                Assert.Null(result);
            }

            [Fact]
            public void NotFound_ReturnsNull()
            {
                var relations = new RelationCollection<string>();
                var result = relations.Get("other");
                Assert.Null(result);
            }

            [Fact]
            public void AddedAndRemoved_ReturnsNull()
            {
                var relations = new RelationCollection<string>();
                var item = "something";
                relations.Add("other", item);
                relations.Remove("other");
                var result = relations.Get("other");
                Assert.Null(result);

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
            public void ReturnsActualCollection()
            {
                var relations = new RelationCollection<string>();
                ICollection<string> expected = new List<string> { "Hi", "these", "are", "strings" };
                relations["other"] = expected;
                ICollection<string> actual = relations.Get("other");
                
                Assert.Same(expected, actual);
            }
        }

        // public class IsSingleRelationMethod
        // {
        //     [Theory]
        //     [InlineData(null)]
        //     [InlineData("")]
        //     [InlineData("  ")]
        //     public void RelNullOrWhitespace_ReturnsFalse(string rel)
        //     {
        //         var relations = new RelationCollection<string>();
        //         bool result = relations.IsSingleRelation(rel);
        //         Assert.False(result);
        //     }

        //     [Fact]
        //     public void AddedSingleRelation_ReturnsTrue()
        //     {
        //         var relations = new RelationCollection<string>();
        //         relations.SingleRelations.Add("other");
        //         bool result = relations.IsSingleRelation("other");
        //         Assert.True(result);
        //     }

        //     [Fact]
        //     public void NotAddedSingleRelation_ReturnsFalse()
        //     {
        //         var relations = new RelationCollection<string>();
        //         relations.SingleRelations.Add("first");
        //         bool result = relations.IsSingleRelation("second");
        //         Assert.False(result);
        //     }
        // }

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
                Assert.Equal(0, relations.Count);
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
                Assert.Throws<ArgumentNullException>(() => relations.Set("other", null as ICollection<string>));
            }

            [Fact]
            public void DeletesIfItemsEmpty()
            {
                var relations = new RelationCollection<string>();
                relations.Add("other", "something");
                
                relations.Set("other", new List<string>());
                var result = relations.Get("other");
                Assert.Null(result);
            }

            [Fact]
            public void AddsNewItem()
            {
                var relations = new RelationCollection<string>();
                var item = "item";
                relations.Set("other", item);
                bool result = relations["other"].Contains(item);
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

        public class SingularRelationsProperty
        {
            [Fact]
            public void Default_Empty()
            {
                var relations = new RelationCollection<string>();
                var result = relations.SingularRelations;
                Assert.NotNull(result);
                Assert.Equal(0, result.Count());
            }
        }
    }
}