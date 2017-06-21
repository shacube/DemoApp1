namespace DemoApp1
{
    public interface IStudentUpdateRequest
    {
        IStudent Student {get; set;}
        long Version {get; set;}
    }
}