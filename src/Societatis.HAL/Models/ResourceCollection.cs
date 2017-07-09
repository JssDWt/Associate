namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    public class ResourceCollection : RelationCollection<Resource>
    {
        private IEnumerable<string> allowedRelations;

        /// <summary>
        /// Initializes a new isntance of a <see cref="ResourceCollection" /> based on the specified links.
        /// </summary>
        /// <param name="links">The links that are the basis of this resource collection.</param>
        public ResourceCollection(LinkCollection links)
        {
            base.SingleRelations = links.SingleRelations;
            this.allowedRelations = links.Relations;
        }

        public override void Add(string rel, Resource resource)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            resource.ThrowIfNull(nameof(resource));

            if (!this.allowedRelations.Contains(rel))
            {
                throw new InvalidOperationException(
                    "Cannot add relation, because there is no corresponding relation in the link collection that is the base of this collection.");
            }

            base.Add(rel, resource);
        }
    }
}