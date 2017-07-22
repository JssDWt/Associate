namespace Societatis.HAL.Tests
{
    public class HALSpecResource : Resource
    {
        public int CurrentlyProcessing { get; set; }
        public int ShippedToday { get; set; }
    }
}