namespace Societatis.HAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    public class RelationCollection : IDictionary<string, IEnumerable<ILink>>
    {
        private CuriesCollection curies = new CuriesCollection();
        private Dictionary<string, List<ILink>> links = new Dictionary<string, List<ILink>>();

        public int Count
        {
            get
            {
                return this.links.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<ILink> this[string rel]
        {
            get
            {
                rel.ThrowIfNullOrWhiteSpace(nameof(rel));
                List<ILink> result = this.Get(rel);
                // TODO: Make sure you cannot cast back to list
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return result.AsReadOnly();
                }
            }
            set
            {
                this.Replace(rel, value);
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                // TODO: Make sure you cannot manipulate the internal keys with this collection.
                return this.links.Keys;
            }
        }

        public ICollection<string> Relations
        {
            get
            {
                return this.Keys;
            }
        }

        public IEnumerable<ILink> AllLinks
        {
            get
            {
                return this.links.Values.SelectMany(link => link);
            }
        }

        public ICollection<IEnumerable<ILink>> Values
        {
            get
            {
                // TODO: Same as keys.
                return this.links.Values.Select(l => l.AsEnumerable()).ToList();
            }
        }

        public ILink Self
        {
            get
            {
                var selfLinks = this.Get("self");
                ILink result = null;
                if (selfLinks != null)
                {
                    result = selfLinks.SingleOrDefault();
                }

                return result;
            }
            set
            {
                value.ThrowIfNull(nameof(value));
                this.Replace("self", new ILink[] { value });
            }
        }

        public CuriesCollection Curies
        {
            get
            {
                return this.curies;
            }
        }

        private List<ILink> Get(string rel)
        {
            List<ILink> result = null;
            if (this.links.ContainsKey(rel))
            {
                result = this.links[rel];
            }

            return result;
        }
        public void Add(KeyValuePair<string, IEnumerable<ILink>> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Add(string rel, IEnumerable<ILink> links)
        {
            // NOTE: rel is checked for null in the Add method below.
            links.ThrowIfNull(nameof(links));
            foreach (ILink link in links.Where(l => l != null))
            {
                this.Add(rel, link);
            }
        }

        public void Add(string rel, ILink link)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            link.ThrowIfNull(nameof(link));

            // NOTE: Non-unique links are allowed. Silly, but allowed.
            // TODO: Verify the link is valid.
            // TODO: Verify the rel matches the link's rel.
            // TODO: throw on multiple self links??
            // TODO: if the rel refers to a curie, make sure the curie exists
            // TODO: if the Link is a curie, make sure it does not yet exist.

            switch (rel)
            {
                case Curie.Relation:
                    var curie = link as Curie;
                    if (curie == null) 
                    {
                        throw new InvalidOperationException("Relation implies that link is a curie, but cannot cast link to a curie.");
                    }
                    
                    this.curies.Add(curie);
                    break;
                default:
                    if (!this.links.ContainsKey(rel))
                    {
                        this.links.Add(rel, new List<ILink>());
                    }

                    this.links[rel].Add(link);
                    break;
            }
        }
        public void Clear()
        {
            // TODO: Also clear selflink and curies??
            this.links.Clear();
        }

        public bool Contains(KeyValuePair<string, IEnumerable<ILink>> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string rel, ILink link)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            link.ThrowIfNull(nameof(link));

            bool result = false;
            if (this.links.ContainsKey(rel))
            {
                result = this.links[rel].Contains(link);
            }

            return result;
        }

        public bool ContainsKey(string rel)
        {
            return this.links.ContainsKey(rel);
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<ILink>>> GetEnumerator()
        {
            foreach (var relation in this.links)
            {
                yield return new KeyValuePair<string, IEnumerable<ILink>>(relation.Key, relation.Value.AsEnumerable());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, IEnumerable<ILink>> item)
        {
            return this.Remove(item.Key);
        }

        public bool Remove(string rel)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));

            bool removed = false;
            if (this.links.ContainsKey(rel))
            {
                removed = this.links.Remove(rel);
            }

            return removed;
        }

        public bool TryGetValue(string rel, out IEnumerable<ILink> links)
        {
            bool success = false;
            links = null;
            List<ILink> innerLinks;

            success = this.links.TryGetValue(rel, out innerLinks);
            if (success)
            {
                links = innerLinks.AsEnumerable();
            }

            return success;
        }

        public void Replace(string rel, IEnumerable<ILink> links)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            links.ThrowIfNull(nameof(links));

            this.Remove(rel);
            this.Add(rel, links);
        }

        public void CopyTo(KeyValuePair<string, IEnumerable<ILink>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}