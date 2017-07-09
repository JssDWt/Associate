namespace Societatis.HAL.Tests
{
    public class TestClass
    {
        public string TestProperty { get; set; }
    }

    public class TestRelationCollection : RelationCollection<TestClass>
    {

    }

    public class HighlyInheritedRelationCollection : TestRelationCollection
    {

    }
}