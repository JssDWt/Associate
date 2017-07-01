public class DataNotFoundException : System.Exception
{
    public DataNotFoundException()
        : base() {}
    public DataNotFoundException(string message)
        : base(message) {}
}