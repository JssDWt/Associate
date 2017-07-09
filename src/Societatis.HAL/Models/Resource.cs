namespace Societatis.HAL
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public abstract class Resource
    {
        public const string LinkPropertyName = "_links";
        public const string EmbeddedPropertyName = "_embedded";
        public Resource()
        {
            this.Links = new LinkCollection();
            this.Embedded = new ResourceCollection(this.Links);
        }

        [JsonProperty(PropertyName = LinkPropertyName)]
        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public LinkCollection Links { get; protected set; }

        [JsonProperty(PropertyName = EmbeddedPropertyName)]
        [JsonConverter(typeof(RelationCollectionJsonConverter))]
        public ResourceCollection Embedded { get; protected set; }
    }

    public class Resource<T> : Resource where T: class
    {
        private T value;

        public Resource(T value)
        {
            this.Value = value;
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
    }
}