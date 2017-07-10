namespace Societatis.HAL
{
    using Newtonsoft.Json;
    public interface IResource
    {
        [JsonProperty("_links")]
        IRelationCollection<ILink> Links { get; }

        [JsonProperty("_embedded")]
        IRelationCollection<IResource> Embedded { get; }
    }
}