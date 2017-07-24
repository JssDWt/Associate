namespace Societatis.HAL
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    /// <summary>
    /// Class describing a resource. Inheriting classes can be serialized as HAL resources.
    /// </summary>
    public abstract class Resource : IResource
    {
        public const string LinksPropertyName = "_links";
        public const string EmbeddedPropertyName = "_embedded";
        public static readonly string[] ReservedProperties = new string[] { LinksPropertyName, EmbeddedPropertyName };

        private IRelationCollection<ILink> links;
        private IRelationCollection<IResource> embedded;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> class.
        /// </summary>
        protected Resource()
        {
            this.Links = new LinkCollection();
            this.Embedded = new ResourceCollection(this.Links);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> class,
        /// using the specified links and embedded resources.
        /// </summary>
        /// <param name="links">The links for the current resource.</param>
        /// <param name="embedded">The embedded resources for the current resource.</param>
        protected Resource(IRelationCollection<ILink> links, IRelationCollection<IResource> embedded)
        {
            this.Links = links;
            this.Embedded = embedded;
        }

        /// <summary>
        /// Gets the links for the current resource. Containing related links to other resources.
        /// </summary>
        
        [JsonConverter(typeof(RelationCollectionJsonConverter), typeof(LinkCollection))]
        [JsonProperty(LinksPropertyName)]
        public IRelationCollection<ILink> Links 
        { 
            get => this.links;
            set
            {
                value.ThrowIfNull(nameof(Links));
                this.links = value;
            }
        }

        /// <summary>
        /// Gets embedded resource, accompanied by the current resource, as a full, partial, 
        /// or inconsistent version of the representations served from the target Uri.
        /// </summary>
        [JsonConverter(typeof(RelationCollectionJsonConverter), typeof(ResourceCollection))]
        [JsonProperty(EmbeddedPropertyName)]
        public IRelationCollection<IResource> Embedded 
        { 
            get => this.embedded;
            set
            {
                value.ThrowIfNull(nameof(Embedded));
                this.embedded = value;
            }
        }

        public virtual bool ShouldSerializeLinks()
        {
            return this.Links != null && this.Links.Count > 0;
        }

        public virtual bool ShouldSerializeEmbedded()
        {
            return this.Embedded != null && this.Embedded.Count > 0;
        }
    }

    /// <summary>
    /// Generic class describing a resource. This class can be used instead of an inheritance stategy for HAL resources.
    /// </summary>
    public class Resource<T> : Resource where T : class
    {
        /// <summary>
        /// Backing field for the Data property.
        /// </summary>
        private T data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> class with the specified data.
        /// </summary>
        /// <param name="data">The data to use as the body of this resource.</param>
        public Resource(T data)
            : base()
        {
            this.Construct(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> class with the specified data,
        /// using the specified links and embedded resources.
        /// </summary>
        /// <param name="data">The data to use as the body of this resource.</param>
        /// <param name="links">The links for the current resource.</param>
        /// <param name="embedded">The embedded resources for the current resource.</param>
        public Resource(
            T data,
            IRelationCollection<ILink> links,
            IRelationCollection<IResource> embedded)
            : base(links, embedded)
        {
            this.Construct(data);
        }

        /// <summary>
        /// Gets or sets the data of the current resource.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the data is set to null.</exception>
        /// <exception cref="ArgumentException">Thrown when the data object implements <see cref="IResource" />.</exception>
        [JsonIgnore]
        public T Data
        {
            get => this.data;
            set
            {
                value.ThrowIfNull(nameof(value));
                if (value is IResource)
                {
                    throw new ArgumentException(
                        $"Cannot set value to an object that implements the {nameof(IResource)} interface. " +
                        "That would break the HAL constraints.", nameof(value));
                }
                this.data = value;
            }
        }

        /// <summary>
        /// Constructs the current object with the specified data.
        /// Used because the constructors cannot be chained.
        /// </summary>
        /// <param name="data">The data to use as the data for the current resource.</param>
        private void Construct(T data)
        {
            this.Data = data;
        }
    }
}