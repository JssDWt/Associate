namespace Societatis.HAL.Tests
{
    public class HALSpecResource : Resource
    {
        public int CurrentlyProcessing { get; set; }
        public int ShippedToday { get; set; }
    }

    public class HALSpecOrder : Resource
    {
        public double Total { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }
}