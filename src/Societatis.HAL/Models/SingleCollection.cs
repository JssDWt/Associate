namespace Societatis.HAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Societatis.Misc;

    public class SingleCollection<T> : ICollection<T>
    {
        public T Item
        {
            get;
            private set;
        }

        private bool isSet = false;

        public int Count => isSet ? 1 : 0;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (this.isSet)
            {
                throw new InvalidOperationException(
                    "Cannot add item, because an item already exists in the collection. " +
                    "Only a single item is allowed.");
            }

            item.ThrowIfNull(nameof(item));
            this.Item = item;
            this.isSet = true;
        }

        public void Clear()
        {
            this.Item = default(T);
            this.isSet = false;
        }

        public bool Contains(T item)
            => this.isSet && this.Item?.Equals(item) == true;

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (this.isSet)
            {
                new T[] { this.Item }.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.isSet)
            {
                yield return this.Item;
            }
            else
            {
                yield break;
            }
        }

        public bool Remove(T item)
        {
            bool removed = false;
            if (this.Contains(item))
            {
                this.Clear();
            }
            
            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}