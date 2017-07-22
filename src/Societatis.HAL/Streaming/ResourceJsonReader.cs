namespace Societatis.HAL.Streaming
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Societatis.Misc;

    internal class ResourceJsonReader : WrappedJsonReader
    {
        /// <summary>
        /// Provides a concrete implementation of the abstract class <see cref="Resource" /> to populate.
        /// </summary>
        public class SerializationResource : Resource { }
        private int objectDepth = 0;
        private JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceJsonReader" /> class,
        /// wrapping the inner <see cref="JsonReader" /> instance.
        /// </summary>
        /// <param name="inner">The <see cref="JsonReader" /> that this one wraps around. 
        /// All methods called on this class will be invoked on the inner reader.</param>
        public ResourceJsonReader(JsonReader inner, JsonSerializer serializer)
            :this (inner, serializer, new SerializationResource())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceJsonReader" /> class,
        /// wrapping the inner <see cref="JsonReader" /> instance. The passed resource object will
        /// be populated with links and embedded resources on the fly while reading json.
        /// </summary>
        /// <param name="inner">The <see cref="JsonReader" /> that this one wraps around. 
        /// All methods called on this class will be invoked on the inner reader.</param>
        /// <param name="resource">The resource that will be populated with links and embedded resources 
        /// while reading json.</param>
        /// <remarks>It is expected that the first start object encountered is the start
        /// of the resource object to read. This reader will invoke the inner reader for anything that is not the
        /// part resource interface.</remarks>
        /// <example>TODO: Add an example of deserializing a resource instance with this class as the reader.</example>
        public ResourceJsonReader(JsonReader inner, JsonSerializer serializer, IResource resource)
            : base(inner)
        {
            resource.ThrowIfNull(nameof(resource));
            serializer.ThrowIfNull(nameof(serializer));

            if (resource.Links == null)
            {
                throw new ArgumentException($"Resource {nameof(IResource.Links)} cannot be null.", "resource");
            }

            if (resource.Embedded == null)
            {
                throw new ArgumentException($"Resource {nameof(IResource.Embedded)} cannot be null.", "resource");
            }

            this.Resource = resource;
            this.serializer = serializer;
        }

        /// <summary>
        /// Gets the resource that is/will be populated with links and embedded resources while json is being read.
        /// </summary>
        public IResource Resource { get; }
        public bool ResourceRead { get; }

        public override bool Read()
        {
            this.ThrowIfDisposed();

            bool success = false;
            if (base.Read())
            {
                success = true;

                if (this.TokenType == JsonToken.StartObject)
                {
                    this.objectDepth++;
                }

                if (this.objectDepth == 1 && this.TokenType == JsonToken.PropertyName)
                {
                    // Currently inside the resource object and handling a property.
                    string propertyName = (string)this.Value;
                    
                    switch (propertyName)
                    {
                        case "_links":
                            var linksType = this.Resource.Links.GetType();
                            var links = (IRelationCollection<ILink>)this.serializer.Deserialize(this.innerReader, linksType);
                            this.Resource.Links = links;

                            // NOTE: Call this.Read(), not base.Read(), because the next property may be another reserved one. This can hardly be called recursion...
                            success = this.Read();
                            break;
                        case "_embedded":
                            var embeddedType = this.Resource.Embedded.GetType();
                            var embedded = (IRelationCollection<IResource>)this.serializer.Deserialize(this.innerReader, embeddedType);
                            this.Resource.Embedded = embedded;

                            // NOTE: Call this.Read(), not base.Read(), because the next property may be another reserved one. This can hardly be called recursion...
                            success = this.Read();
                            break;
                        default:
                            // Do nothing.
                            break;
                    }
                }
            }
            
            return success;
        }

        private void ThrowIfDisposed()
        {
            if (base.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}