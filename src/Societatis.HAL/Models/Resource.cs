namespace Societatis.HAL
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public abstract class Resource : IResource
    {
        public Resource()
        {
            this.Links = new LinkCollection();
            this.Embedded = new ResourceCollection(this.Links);
        }

        public Resource(IRelationCollection<ILink> links, IRelationCollection<IResource> embedded)
        {
            links.ThrowIfNull(nameof(links));
            embedded.ThrowIfNull(nameof(embedded));

            this.Links = links;
            this.Embedded = embedded;
        }

        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public IRelationCollection<ILink> Links { get; }

        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public IRelationCollection<IResource> Embedded { get; }
    }

    public class Resource<T> : Resource where T: class
    {
        private T value;

        public Resource(T value)
            :base()
        {
            this.Construct(value);
        }

        public Resource(
            T value, 
            IRelationCollection<ILink> links, 
            IRelationCollection<IResource> embedded)
            : base(links, embedded)
        {
            this.Construct(value);
        }

        [JsonIgnore]
        public T Value 
        { 
            get => this.value;
            set
            {
                value.ThrowIfNull(nameof(value));
                this.value = value;
            }
        }

        private void Construct(T value)
        {
            this.Value = value;
        }
    }
}