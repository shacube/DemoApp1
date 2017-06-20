namespace DemoApp1
{
    public interface ISearchRequest
    {
        int Offset {get; set;}
        int PageSize {get; set;}
        string Term {get; set;}
    }

    public interface IClassSearchRequest : ISearchRequest
    {
        string Code {get; set;}
        string Title {get; set;}
        string Description {get; set;}        
    }

    public interface IStudentSearchRequest : ISearchRequest
    {
        string FirstName {get; set;}
        string LastName {get; set;}        
    }
}