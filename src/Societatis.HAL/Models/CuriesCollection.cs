namespace Societatis.HAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Societatis.Misc;

    public class CuriesCollection : ICollection<Curie>
    {
        private List<Curie> curies = new List<Curie>();

        public int Count => this.curies.Count;

        public bool IsReadOnly => false;

        public void Add(Curie curie)
        {
            curie.ThrowIfNull(nameof(curie));
            
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Curie curie)
        {
            curie.ThrowIfNull(nameof(curie));
            return this.curies.Contains(curie);
        }

        public void CopyTo(Curie[] array, int arrayIndex)
        {
            array.ThrowIfNull(nameof(array));
            this.curies.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Curie> GetEnumerator()
        {
            return this.curies.GetEnumerator();
        }

        public bool Remove(Curie curie)
        {
            curie.ThrowIfNull(nameof(curie));
            return this.curies.Remove(curie);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}