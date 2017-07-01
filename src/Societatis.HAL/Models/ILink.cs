namespace Societatis.HAL
{
    using System;

    public interface ILink
    {
        /// <summary>
        /// Gets or sets the location of the resource the link points at.
        /// </summary>
        Uri HRef { get;}

        /// <summary>
        /// Gets or sets a value indicating whether the HRef property is a URI template.
        /// </summary>
        bool Templated { get; }

        /// <summary>
        /// Gets or sets a hint to indicate the media type expected when dereferencing the target resource by the HRef Uri.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets or sets a URL indicating that the link is to be deprecated at a future date.
        /// The URL provides further information about the deprecation.
        /// </summary>
        Uri Deprecation { get; }

        /// <summary>
        /// Gets or sets a value that may be used as a secondary key for selecting Link Objects which share the same relation type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets a URI that hints about the profile of the target resource.
        /// </summary>
        Uri Profile { get; }

        /// <summary>
        /// Gets or sets a human-readable identifier for the link.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets or sets a string that indicates the language of the target resource.
        /// </summary>
        string HrefLang { get; }
    }
}