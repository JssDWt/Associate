namespace Societatis.HAL
{
    using Newtonsoft.Json;
    public interface IResource
    {
        [JsonIgnore]
        IRelationCollection<ILink> Links { get; }

        [JsonIgnore]
        IRelationCollection<IResource> Embedded { get; }
    }
}
