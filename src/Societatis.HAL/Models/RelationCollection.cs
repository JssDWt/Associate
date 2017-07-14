namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    /// <summary>
    /// Class describing a HAL relation collection.
    /// </summary>
    public class RelationCollection<T> : IRelationCollection<T>
    {
        /// <summary>
        /// Backing field for the SingleRelations property.
        /// </summary>
        private ICollection<string> singleRelations = new HashSet<string>();

        /// <summary>
        /// Field containing all relations and their items.
        /// </summary>
        private Dictionary<string, List<T>> relations = new Dictionary<string, List<T>>();

        /// <summary>
        /// Gets the number of distinct relations.
        /// </summary>
        public virtual int Count
        {
            get => this.relations.Count;
        }

        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        public virtual int ItemCount
        {
            get => this.relations.Values
                                 .Select(rel => rel.Count)
                                 .Sum();
        }

        /// <summary>
        /// Gets or sets the items in the specified relation.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>All items in the specified relation.</returns>
        public virtual IEnumerable<T> this[string rel]
        {
            get => this.Get(rel);
            set => this.Set(rel, value);
        }

        /// <summary>
        /// Gets the names of the relations.
        /// </summary>
        public virtual IEnumerable<string> Relations
        {
            get
            {
                foreach (var relation in this.relations.Keys)
                {
                    yield return relation;
                }
            }
        }

        /// <summary>
        /// Gets the collection of relations that should be treated as single relations.
        /// A relation that occurs in this collection can only contain a single item.
        /// </summary>
        public virtual ICollection<string> SingleRelations
        {
            get
            {
                return this.singleRelations;
            }

            protected set
            {
                value.ThrowIfNull(nameof(value));
                this.singleRelations = value;
            }
        }

        /// <summary>
        /// Gets all items in the collection, regardless of the relation they are in.
        /// </summary>
        public virtual IEnumerable<T> All
        {
            get
            {
                return this.relations.Values.SelectMany(item => item);
            }
        }

        /// <summary>
        /// Adds all the specified items to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to add the items to.</param>
        /// <param name="items">The items to add to the relation.</param>
        /// <exception cref="ArgumentNullException">Thrown when items is null</exception>
        /// <exception cref="ArgumentException">Thrown when the specified rel is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Thrown when only a single item is allowed in the specified relation
        /// and an item in the relation already exists.</exception>
        public virtual void Add(string rel, IEnumerable<T> items)
        {
            // NOTE: rel is checked for null in the Add method below.
            items.ThrowIfNull(nameof(items));
            foreach (T item in items.Where(i => i != null))
            {
                this.Add(rel, item);
            }
        }

        /// <summary>
        /// Adds the specified item to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to add the item to.</param>
        /// <param name="item">The item to add to the relation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the specified item is null</exception>
        /// <exception cref="ArgumentException">Thrown when the specified rel is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Thrown when only a single item is allowed in the specified relation
        /// and an item in the relation already exists.</exception>
        public virtual void Add(string rel, T item)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            item.ThrowIfNull(nameof(item));

            if (this.relations.ContainsKey(rel))
            {
                // This means there is already an item in this relation. Throw if only a single item may occur.
                if(this.SingleRelations.Contains(rel))
                {
                    throw new InvalidOperationException(
                        "Cannot add relation item. An item in this relation already exists " +
                        "and multiple items in this relation are not allowed, because the relation occurs " +
                        $" in {nameof(this.SingleRelations)}.");
                }
            }
            else 
            {
                // The relation does not yet exist. Add it.
                this.relations.Add(rel, new List<T>());
            }

            this.relations[rel].Add(item);
        }

        /// <summary>
        /// Clears all relations and items.
        /// </summary>
        public virtual void Clear()
        {
            this.relations.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the specified relation occurs.
        /// </summary>
        /// <param name="relation">The relation to contain.</param>
        /// <returns>A value indicating whether the current collection contains the specified relation.</returns>
        public virtual bool Contains(string rel)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(rel))
            {
                result = this.relations.ContainsKey(rel);
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the specified relation contains the specified item.
        /// </summary>
        /// <param name="rel">The relation that would contain the item.</param>
        /// <param name="item">The item that would be in the relation.</param>
        /// <returns>A value indicating whether the item is in the relation.</returns>
        public virtual bool Contains(string rel, T item)
        {
            bool result = false;

            if (item != null
                && this.Contains(rel))
            {
                result = this.relations[rel].Contains(item);
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the specified item occurs in the collection.
        /// </summary>
        /// <param name="item">The item that would be in the collection.</param>
        /// <returns>A value indicating whether the item is in the collection.</returns>
        public virtual bool Contains(T item)
        {
            bool result = false;

            if (item != null)
            {
                result = this.Relations.Any(rel => this.Contains(rel, item));
            }

            return result;
        }

        /// <summary>
        /// Returns all items in the specified relation.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>All items in the specified relation.</returns>
        public IEnumerable<T> Get(string rel)
        {
            IEnumerable<T> result = null;
            if (this.Contains(rel))
            {
                result = this.relations[rel];
            }

            if (result == null)
            {
                yield break;
            }
            else
            {
                foreach (var item in result)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified rel occurs in the SingleRelations collection.
        /// </summary>
        /// <param name="rel">The relation to check.</param>
        /// <returns>A value indicating whether the specified relation occurs in the SingleRelations collection.</returns>
        public virtual bool IsSingleRelation(string relation)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(relation))
            {
                result = this.SingleRelations.Contains(relation);
            }

            return result;
        }

        /// <summary>
        /// Sets the items in the specified relation, replacing existing items in the relation if they exist.
        /// </summary>
        /// <param name="rel">The relation to set the items for.</param>
        /// <param name="items">The items to set in the relation.</param>
        /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
        /// <exception cref="ArgumentException">Thrown when rel is null or whitespace.</exception>
        public virtual void Set(string rel, IEnumerable<T> items)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            items.ThrowIfNull(nameof(items));

            this.Remove(rel);
            this.Add(rel, items);
        }

        /// <summary>
        /// Sets the item in the specified relation, replacing existing items in the relation if they exist.
        /// </summary>
        /// <param name="rel">The relation to set the item for.</param>
        /// <param name="items">The item to set in the relation.</param>
        /// <exception cref="ArgumentNullException">Thrown when item is null.</exception>
        /// <exception cref="ArgumentException">Thrown when rel is null or whitespace.</exception>
        public virtual void Set(string rel, T item)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            item.ThrowIfNull(nameof(item));

            this.Remove(rel);
            this.Add(rel, item);
        }

        /// <summary>
        /// Removes the specified relation, and all its items from the collection.
        /// </summary>
        /// <param name="relation">The relation to remove.</param>
        /// <returns>A value indicathing whether the relation was removed from the collection in this call.</returns>
        public virtual bool Remove(string rel)
        {
            bool removed = false;

            if (!string.IsNullOrWhiteSpace(rel)
                && this.relations.ContainsKey(rel))
            {
                removed = this.relations.Remove(rel);
            }

            return removed;
        }
    }
}