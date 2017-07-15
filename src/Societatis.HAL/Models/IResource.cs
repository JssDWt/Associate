namespace Societatis.HAL
{
    using Newtonsoft.Json;

    /// <summary>
    /// Interface describing a HAL resource.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the links for the current resource. Containing related links to other resources.
        /// </summary>
        [JsonIgnore]
        IRelationCollection<ILink> Links { get; }

        /// <summary>
        /// Gets embedded resource, accompanied by the current resource, as a full, partial, 
        /// or inconsistent version of the representations served from the target Uri.
        /// </summary>
        [JsonIgnore]
        IRelationCollection<IResource> Embedded { get; }
    }
}
