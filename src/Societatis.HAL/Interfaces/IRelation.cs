namespace Societatis.HAL
{
    using System.Collections.Generic;

    public interface IRelation<T>
    {
        string Relation { get; }
        bool IsSingular { get; }
        ICollection<T> Items { get; }
    }
}