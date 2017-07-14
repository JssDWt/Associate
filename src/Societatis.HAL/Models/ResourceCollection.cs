namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    /// <summary>
    /// Class describing a collection of embedded resources.
    /// </summary>
    public class ResourceCollection : RelationCollection<IResource>
    {
        /// <summary>
        /// Field containing the allowed relations for the current object.
        /// The allowed relations are the relations of the links object passed into the constructor.
        /// </summary>
        private IEnumerable<string> allowedRelations;

        /// <summary>
        /// Initializes a new instance of a <see cref="ResourceCollection" /> based on the specified links.
        /// </summary>
        /// <param name="links">The links that are the basis of this resource collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when links is null.</exception>
        /// <exception cref="ArgumentException">Thrown when either the SingleRelations or Relations property
        /// of the specified links is null.</exception>
        public ResourceCollection(IRelationCollection links)
        {
            links.ThrowIfNull(nameof(links));
            if (links.SingleRelations == null)
            {
                throw new ArgumentException($"links {nameof(links.SingleRelations)} cannot be null.", nameof(links));
            }

            if (links.Relations == null)
            {
                throw new ArgumentException($"links {nameof(links.Relations)} cannot be null.", nameof(links));
            }

            base.SingleRelations = links.SingleRelations;
            this.allowedRelations = links.Relations;
        }

        /// <summary>
        /// Adds the specified resource to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to the resource.</param>
        /// <param name="resource">The resource.</param>
        public override void Add(string rel, IResource resource)
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