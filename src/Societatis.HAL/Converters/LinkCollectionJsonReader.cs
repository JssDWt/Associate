namespace Societatis.HAL.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public class LinkCollectionJsonReader : RelationCollectionJsonReader<ILink>
    {
        /// <summary>
        /// Backing field for the <see cref="LinkType" /> property.
        /// </summary>
        private Type linkType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollectionJsonReader" /> class.
        /// </summary>
        public LinkCollectionJsonReader()
        {
            this.LinkType = typeof(Link);
        }

        /// <summary>
        /// Gets or sets the type of link to use for deserialized links.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the provided type is not a concrete type.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided type does not implement <see cref="ILink" />.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided type does not have a default constructor.</exception>
        public Type LinkType
        {
            get => this.linkType;
            set
            {
                value.ThrowIfNull(nameof(value));
                var typeInfo = value.GetTypeInfo();
                if (!typeInfo.IsConcreteType())
                {
                    throw new ArgumentException("Type should be a concrete type.", nameof(value));
                }

                if (!typeof(ILink).GetTypeInfo().IsAssignableFrom(typeInfo))
                {
                    throw new ArgumentException($"Type must implement {nameof(ILink)}.", nameof(value));
                }

                // TODO: Support Uri constructor as well.
                var defaultConstructor = typeInfo.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor == null)
                {
                    throw new ArgumentException($"Type must have a default constructor.");
                }

                this.linkType = value;
            }
        }

        protected override Type GetTypeForRelation(string relation)
        {
            return this.LinkType;
        }
    }
}