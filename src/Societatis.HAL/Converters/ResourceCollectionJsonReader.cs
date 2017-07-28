namespace Societatis.HAL.Converters
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public class ResourceCollectionJsonReader : RelationCollectionJsonReader<IResource>
    {
        protected override Type GetTypeForRelation(string relation)
        {
            throw new NotImplementedException();
        }
    }
}