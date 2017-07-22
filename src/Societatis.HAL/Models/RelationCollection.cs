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
        /// Field containing all relations and their items.
        /// </summary>
        private Dictionary<string, ICollection<T>> relations = new Dictionary<string, ICollection<T>>();

        /// <summary>
        /// Backing field for the DefaultValueProvider property.
        /// </summary>
        private Func<ICollection<T>> defaultValueProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationCollection" /> class.
        /// </summary>
        public RelationCollection() // TODO: Make this something other than a list...
            : this(() => new List<T>()) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationCollection" /> class,
        /// using the specified valueprovider as a default value provider for the relation values.
        /// </summary>
        /// <param name="defaultValueProvider">Function returning a collection for relation values.</param>
        public RelationCollection(Func<ICollection<T>> defaultValueProvider)
        {
            this.DefaultValueProvider = defaultValueProvider;
        }

        /// <summary>
        /// Gets or sets a function providing the default collection when a new relation is requested.
        /// </summary>
        public Func<ICollection<T>> DefaultValueProvider 
        { 
            get => this.defaultValueProvider; 
            set
            {
                value.ThrowIfNull(nameof(value));
                this.defaultValueProvider = value;
            }
        }

        /// <summary>
        /// Gets the number of relations in the collection.
        /// </summary>
        public virtual int Count
        {
            get => this.relations.Count;
        }

        /// <summary>
        /// Gets or sets the item collection of the specified relation.
        /// The value returned is never null. If a relation is requested that is not currently present in the collection,
        /// it is added with the <see cref="DefaultValueProvider" /> and returned.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>Collection of items in the specified relation.</returns>
        /// <example> 
        /// This sample shows how to add items to a collection. Below code will not throw an exception when the relation does not exist.
        /// <code>
        /// var relations = new RelationCollection<object>();
        /// relations["myRelation"].Add(new object()); // does not throw.
        /// </code>
        /// </example>
        public virtual ICollection<T> this[string rel]
        {
            get
            {
                ICollection<T> result = this.Get(rel);
                if (result == null)
                {
                    result = this.DefaultValueProvider();
                    this.Set(rel, result);
                }

                return result;
            }
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
        /// Adds all the specified items to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to add the items to.</param>
        /// <param name="items">The items to add to the relation.</param>
        /// <exception cref="ArgumentNullException">Thrown when items is null</exception>
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
        public virtual void Add(string rel, T item)
        {
            this[rel].Add(item);
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
        /// Returns all items in the specified relation, or null if the relation is not found.
        /// </summary>
        /// <param name="rel">The relation to get items for.</param>
        /// <returns>All items in the specified relation.</returns>
        public ICollection<T> Get(string rel)
        {
            ICollection<T> result = null;

            if (this.Contains(rel))
            {
                result = this.relations[rel];
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
        public virtual void Set(string rel, ICollection<T> items)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            items.ThrowIfNull(nameof(items));

            this.relations[rel] = items;
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