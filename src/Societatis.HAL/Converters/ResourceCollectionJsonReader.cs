namespace Societatis.HAL.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using Societatis.Misc;

    public class ResourceCollectionJsonReader : RelationCollectionJsonReader<IResource>
    {
        public ResourceCollectionJsonReader()
        {
            this.RelationTypes = new Dictionary<string, Type>();
        }

        public IDictionary<string, Type> RelationTypes { get; set; }

        protected override Type GetTypeForRelation(string relation)
        {
            return this.RelationTypes[relation];
        }
    }
}