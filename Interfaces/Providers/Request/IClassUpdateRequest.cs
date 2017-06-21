namespace DemoApp1
{
    public interface IClassUpdateRequest
    {
        IClass ClassObj {get; set;}
        long Version {get; set;}
    }
}