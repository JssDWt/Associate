namespace Societatis.HAL
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the non-generic interface for a relation collection.
    /// </summary>
    public interface IRelationCollection
    {
        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        int ItemCount {get;}

        /// <summary>
        /// Gets the number of distinct relations.
        /// </summary>
        int Count {get;}

        /// <summary>
        /// Gets the names of the relations.
        /// </summary>
        IEnumerable<string> Relations {get;}

        /// <summary>
        /// Gets the collection of relations that should be treated as single relations.
        /// A relation that occurs in this collection can only contain a single item.
        /// </summary>
        ICollection<string> SingleRelations {get;}

        /// <summary>
        /// Clears all relations and items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a value indicating whether the specified relation occurs.
        /// </summary>
        /// <param name="relation">The relation to contain.</param>
        /// <returns>A value indicating whether the current collection contains the specified relation.</returns>
        bool Contains(string relation);

        /// <summary>
        /// Gets a value indicating whether the specified rel occurs in the SingleRelations collection.
        /// </summary>
        /// <param name="rel">The relation to check.</param>
        /// <returns>A value indicating whether the specified relation occurs in the SingleRelations collection.</returns>
        bool IsSingleRelation(string rel);

        /// <summary>
        /// Removes the specified relation, and all its items from the collection.
        /// </summary>
        /// <param name="relation">The relation to remove.</param>
        /// <returns>A value indicathing whether the relation was removed from the collection in this call.</returns>
        bool Remove(string relation);
    }

    /// <summary>
    /// Generic interface representing a relation collection for items of a specific type.
    /// </summary>
    public interface IRelationCollection<T> : IRelationCollection
    {
        /// <summary>
        /// Gets or sets the items in the specified relation.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>All items in the specified relation.</returns>
        IEnumerable<T> this[string rel] {get; set;}

        /// <summary>
        /// Gets all items in the collection, regardless of the relation they are in.
        /// </summary>
        IEnumerable<T> All {get;}

        /// <summary>
        /// Adds all the specified items to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to add the items to.</param>
        /// <param name="items">The items to add to the relation.</param>
        void Add(string rel, IEnumerable<T> items);

        /// <summary>
        /// Adds the specified item to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to add the item to.</param>
        /// <param name="item">The item to add to the relation.</param>
        void Add(string rel, T item);

        /// <summary>
        /// Gets a value indicating whether the specified relation contains the specified item.
        /// </summary>
        /// <param name="rel">The relation that would contain the item.</param>
        /// <param name="item">The item that would be in the relation.</param>
        /// <returns>A value indicating whether the item is in the relation.</returns>
        bool Contains(string rel, T item);

        /// <summary>
        /// Gets all items in the specified relation.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>All items in the specified relation.</returns>
        IEnumerable<T> Get(string rel);

        /// <summary>
        /// Sets the items in the specified relation, replacing existing items in the relation if they exist.
        /// </summary>
        /// <param name="rel">The relation to set the items for.</param>
        /// <param name="items">The items to set in the relation.</param>
        void Set(string rel, IEnumerable<T> items);

        /// <summary>
        /// Sets the item in the specified relation, replacing existing items in the relation if they exist.
        /// </summary>
        /// <param name="rel">The relation to set the item for.</param>
        /// <param name="item">The item to set in the relation.</param>
        void Set(string rel, T item);
    }
}