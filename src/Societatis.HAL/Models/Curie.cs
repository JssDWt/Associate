namespace Societatis.HAL
{
    using System;
    using Societatis.Misc;

    /// <summary>
    /// Class representing a curie, or compact URI.
    /// </summary>
    public class Curie : Link
    {
        /// <summary>
        /// The relation under which curies are stored in an <see cref="IRelationCollection" />.
        /// </summary>
        public const string Relation = "curies";

        /// <summary>
        /// Initializes a new instance of the <see cref="Curie" /> class.
        /// </summary>
        /// <param name="name">The name of the curie.</param>
        /// <param name="template">The template Uri for the curie.</param>
        public Curie(string name, Uri template)
            : base(template)
        {
            this.Templated = true;
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the HRef property is a URI template.
        /// Will return true, since a curie is always templated.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the value is set to anything other than true.</exception>
        public override bool? Templated
        {
            get => base.Templated;
            set
            {
                if (value != true)
                {
                    throw new InvalidOperationException("A curie is always templated.");
                }

                base.Templated = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the curie, that may be used as a secondary key for selecting curies.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the set value is null or whitespace.</exception>
        public override string Name
        {
            get => base.Name;
            set
            {
                value.ThrowIfNullOrWhiteSpace(nameof(value));
                base.Name = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified link is a valid curie.
        /// </summary>
        /// <param name="link">The link to check.</param>
        /// <returns>A value indicating whether the link is a valid curie.</returns>
        public static bool IsValidCurie(ILink link)
        {
            if (link == null) return false;

            bool result;

            if (link is Curie)
            {
                result = true;
            }
            
            // TODO: Improve this method to use the curie specification
            else if (link.Templated == true
                && link.Name != null
                && link.HRef.ToString().Contains("{rel}"))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}