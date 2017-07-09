namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    public abstract class RelationCollection<T> : IRelationCollection<T> where T: class
    {
        private ICollection<string> singleRelations = new List<string>() { "self" };
        private Dictionary<string, List<T>> relations = new Dictionary<string, List<T>>();

        public virtual int RelationCount
        {
            get => this.relations.Count;
        }

        public virtual int Count
        {
            get => this.relations.Values
                                 .Select(rel => rel.Count)
                                 .Sum();
        }

        public virtual IEnumerable<T> this[string rel]
        {
            get => this.Get(rel);
            set => this.Set(rel, value);
        }

        public virtual IEnumerable<string> Relations
        {
            get
            {
                return this.relations.Keys.AsEnumerable();
            }
        }

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

        public virtual IEnumerable<T> All
        {
            get
            {
                return this.relations.Values.SelectMany(item => item);
            }
        }

        public virtual void Add(string rel, IEnumerable<T> items)
        {
            // NOTE: rel is checked for null in the Add method below.
            items.ThrowIfNull(nameof(items));
            foreach (T item in items.Where(i => i != null))
            {
                this.Add(rel, item);
            }
        }

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
                        $"Cannot add relation item. An item in this relation already exists and multiple items in this relation are not allowed, because the relation occurs in {nameof(this.SingleRelations)}.");
                }
            }
            else 
            {
                // The relation does not yet exist. Add it.
                this.relations.Add(rel, new List<T>());
            }

            this.relations[rel].Add(item);
        }

        public virtual void Clear()
        {
            this.relations.Clear();
        }

        public virtual bool Contains(string rel)
        {
            return this.relations.ContainsKey(rel);
        }

        public virtual bool Contains(string rel, T item)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            item.ThrowIfNull(nameof(item));

            bool result = false;
            if (this.Contains(rel))
            {
                result = this.relations[rel].Contains(item);
            }

            return result;
        }

        public virtual bool Contains(T item)
        {
            return this.Relations.Any(rel => this.Contains(rel, item));
        }

        public IEnumerable<T> Get(string rel)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            var result = this.Get(rel);

            // TODO: Make sure you cannot cast back to list
            return result == null ? result : result.AsEnumerable();
        }

        public virtual bool IsSingleRelation(string relation)
        {
            relation.ThrowIfNullOrWhiteSpace(nameof(relation));
            return this.SingleRelations.Contains(relation);
        }

        public virtual void Set(string rel, IEnumerable<T> items)
        {
            this.Replace(rel, items);
        }

        public virtual void Set(string rel, T item)
        {
            this.Replace(rel, item);
        }

        public virtual bool Remove(string rel)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));

            bool removed = false;
            if (this.relations.ContainsKey(rel))
            {
                removed = this.relations.Remove(rel);
            }

            return removed;
        }

        public virtual void Replace(string rel, IEnumerable<T> items)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            items.ThrowIfNull(nameof(items));

            this.Remove(rel);
            this.Add(rel, items);
        }

        public virtual void Replace(string rel, T item)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            item.ThrowIfNull(nameof(item));

            this.Remove(rel);
            this.Add(rel, item);
        }
    }
}