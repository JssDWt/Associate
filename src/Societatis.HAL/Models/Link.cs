namespace Societatis.HAL
{
    using System;
    using Societatis.Misc;
    
    public class Link : ILink
    {
        public Link(Uri href)
        {
            // NOTE: Argument validation is done in the setter of the property.
            this.HRef = href;
        }

        private Uri href;

        // TODO: Make sure this is either a Uri or a Uri template.
        /// <summary>
        /// Gets or sets the location of the resource the link points at.
        /// </summary>
        public virtual Uri HRef 
        { 
            get { return this.href; }
            set 
            {
                value.ThrowIfNull(nameof(value));
                this.href = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the HRef property is a URI template.
        /// </summary>
        public virtual bool? Templated { get; set; }

        /// <summary>
        /// Gets or sets a hint to indicate the media type expected when dereferencing the target resource by the HRef Uri.
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Gets or sets a URL indicating that the link is to be deprecated at a future date.
        /// The URL provides further information about the deprecation.
        /// </summary>
        public virtual Uri Deprecation { get; set; }

        /// <summary>
        /// Gets or sets a value that may be used as a secondary key for selecting Link Objects which share the same relation type.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets a URI that hints about the profile of the target resource.
        /// </summary>
        public virtual Uri Profile { get; set; }

        /// <summary>
        /// Gets or sets a human-readable identifier for the link.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets a string that indicates the language of the target resource.
        /// </summary>
        public virtual string HrefLang { get; set; }
    }
}