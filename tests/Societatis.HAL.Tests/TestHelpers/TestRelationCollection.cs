using System;
using System.Collections;
using System.Collections.Generic;

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

    public class ConcreteLinkCollection : IRelationCollection<ConcreteLink>
    {
        public ICollection<ConcreteLink> this[string rel] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public IEnumerable<string> RelationNames => throw new NotImplementedException();

        public void Add(string rel, IEnumerable<ConcreteLink> items)
        {
            throw new NotImplementedException();
        }

        public void Add(string rel, ConcreteLink item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string relation)
        {
            throw new NotImplementedException();
        }

        public ICollection<ConcreteLink> Get(string rel)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRelation<ConcreteLink>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void MarkSingular(string relation)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string relation)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ICollection<ConcreteLink> items)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ConcreteLink item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class AbstractLinkCollection : IRelationCollection<ILink>
    {
        public ICollection<ILink> this[string rel] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public IEnumerable<string> RelationNames => throw new NotImplementedException();

        public void Add(string rel, IEnumerable<ILink> items)
        {
            throw new NotImplementedException();
        }

        public void Add(string rel, ILink item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string relation)
        {
            throw new NotImplementedException();
        }

        public ICollection<ILink> Get(string rel)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRelation<ILink>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void MarkSingular(string relation)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string relation)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ICollection<ILink> items)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ILink item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class ConcreteResourceCollection : IRelationCollection<IResource>
    {
        public ICollection<IResource> this[string rel] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public IEnumerable<string> RelationNames => throw new NotImplementedException();

        public void Add(string rel, IEnumerable<IResource> items)
        {
            throw new NotImplementedException();
        }

        public void Add(string rel, IResource item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string relation)
        {
            throw new NotImplementedException();
        }

        public ICollection<IResource> Get(string rel)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRelation<IResource>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void MarkSingular(string relation)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string relation)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ICollection<IResource> items)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, IResource item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class AbstractResourceCollection : IRelationCollection<IResource>
    {
        public ICollection<IResource> this[string rel] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public IEnumerable<string> RelationNames => throw new NotImplementedException();

        public void Add(string rel, IEnumerable<IResource> items)
        {
            throw new NotImplementedException();
        }

        public void Add(string rel, IResource item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string relation)
        {
            throw new NotImplementedException();
        }

        public ICollection<IResource> Get(string rel)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRelation<IResource>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void MarkSingular(string relation)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string relation)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, ICollection<IResource> items)
        {
            throw new NotImplementedException();
        }

        public void Set(string rel, IResource item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}