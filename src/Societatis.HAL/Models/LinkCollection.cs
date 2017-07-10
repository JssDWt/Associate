namespace Societatis.HAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Societatis.Misc;

    public class LinkCollection : RelationCollection<ILink>
    {
        public ILink Self
        {
            get
            {
                var self = this["self"];
                return self == null ? null : self.SingleOrDefault();
            }
            set
            {
                value.ThrowIfNull(nameof(value));
                this.Set("self", value);
            }
        }

        public IEnumerable<Curie> Curies
        {
            get
            {
                var curies = this["curies"];
                return curies == null ? new List<Curie>().AsEnumerable() : curies.Cast<Curie>();
            }
        }

        public override void Add(string rel, ILink link)
        {
            rel.ThrowIfNullOrWhiteSpace(nameof(rel));
            link.ThrowIfNull(nameof(link));

            // NOTE: Non-unique links are allowed. Silly, but allowed.
            // TODO: Verify the link is valid.
            // TODO: if the rel refers to a curie, make sure the curie exists

            if (rel == "curies" && !Curie.IsCurie(link))
            {
                throw new InvalidOperationException("The specified relation implies the link is a curie, but it is not a valid curie.");
            }
            
            base.Add(rel, link);
        }
    }
}