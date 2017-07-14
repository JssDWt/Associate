namespace Societatis.HAL
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Societatis.Misc;

    /// <summary>
    /// Class describing a resource. Inheriting classes can be serialized as HAL resources.
    /// </summary>
    public abstract class Resource : IResource
    {
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
            links.ThrowIfNull(nameof(links));
            embedded.ThrowIfNull(nameof(embedded));

            this.Links = links;
            this.Embedded = embedded;
        }

        /// <summary>
        /// Gets the links for the current resource. Containing related links to other resources.
        /// </summary>
        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public IRelationCollection<ILink> Links { get; }

        /// <summary>
        /// Gets embedded resource, accompanied by the current resource, as a full, partial, 
        /// or inconsistent version of the representations served from the target Uri.
        /// </summary>
        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public IRelationCollection<IResource> Embedded { get; }
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
        [JsonIgnore]
        public T Data
        {
            get => this.data;
            set
            {
                value.ThrowIfNull(nameof(value));
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