namespace Societatis.HAL.Tests
{
    public class TestResource : Resource
    {
        public TestResource()
            :base()
        {
            
        }

        public TestResource(IRelationCollection<ILink> links, IRelationCollection<IResource> embedded)
            :base(links, embedded)
        {
            
        }
        
        public string TestProperty { get; set; }
    }
}