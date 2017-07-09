namespace Societatis.HAL
{
    using System.Collections.Generic;

    public interface IRelationCollection
    {
        int Count {get;}
        int RelationCount {get;}
        IEnumerable<string> Relations {get;}
        ICollection<string> SingleRelations {get;}
        void Clear();
        bool Contains(string relation);
        bool IsSingleRelation(string rel);
        bool Remove(string relation);
        
    }

    public interface IRelationCollection<T> : IRelationCollection where T: class
    {
        IEnumerable<T> this[string rel] {get; set;}
        IEnumerable<T> All {get;}
        void Add(string rel, IEnumerable<T> items);
        void Add(string rel, T item);
        bool Contains(string rel, T item);
        IEnumerable<T> Get(string rel);
        void Replace(string rel, IEnumerable<T> items);
        void Replace(string rel, T item);
        void Set(string rel, IEnumerable<T> items);
        void Set(string rel, T item);
    }
}