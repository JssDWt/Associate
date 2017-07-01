namespace Societatis.HAL
{
    using System.Collections.Generic;

    public abstract class Resource
    {
        public Resource()
        {
            this.Links = new RelationCollection();
            this.Embedded = new List<Resource>();
        }

        public RelationCollection Links { get; private set; }
        public ICollection<Resource> Embedded { get; private set; }
    }
}