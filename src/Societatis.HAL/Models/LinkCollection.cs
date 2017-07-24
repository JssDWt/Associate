namespace Societatis.HAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    /// <summary>
    /// Class representing a relation collection of links.
    /// </summary>
    public class LinkCollection : RelationCollection<ILink>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection" /> class.
        /// </summary>
        public LinkCollection()
        {
            base["self"] = new SingularCollection<ILink>();
        }

        /// <summary>
        /// Gets or sets the self link of this link collection.
        /// </summary>
        public ILink Self
        {
            get => this["self"].FirstOrDefault();
            set
            {
                value.ThrowIfNull(nameof(value));
                this.Set("self", value);
            }
        }

        /// <summary>
        /// Gets the curies that links in the collection may refer to.
        /// </summary>
        /// <remarks>In order to add a curie to the collection, use the Add method with the 'curies' relation.</remarks>
        public ICollection<ILink> Curies
        {
            get => this["curies"];
            set => this.Set("curies", value);
        }

        /// <summary>
        /// Adds the specified link to the specified relation.
        /// </summary>
        /// <param name="rel">The relation to the link.</param>
        /// <param name="link">The link.</param>
        public override void Add(string rel, ILink link)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            link.ThrowIfNull(nameof(link));

            // NOTE: Non-unique links are allowed. Silly, but allowed.
            // TODO: Verify the link is valid.
            // TODO: if the rel refers to a curie, make sure the curie exists

            if (rel == "curies" && !Curie.IsValidCurie(link))
            {
                throw new InvalidOperationException("The specified relation implies the link is a curie, but it is not a valid curie.");
            }
            
            base.Add(rel, link);
        }
    }
}