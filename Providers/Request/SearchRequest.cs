using Nest;
using Newtonsoft.Json;

namespace DemoApp1.Providers
{
    public abstract class SearchRequest : ISearchRequest
    {
        [JsonProperty("offset", Required = Required.DisallowNull)]                
        public int Offset {get; set;}
        
        [JsonProperty("pagesize", Required = Required.DisallowNull)]
        public int PageSize {get; set;}

        [JsonProperty("term", Required = Required.DisallowNull)]
        public string Term {get; set;}
    }

    public class StudentSearchRequest : SearchRequest, IStudentSearchRequest
    {       
        
        [JsonProperty("firtsname", Required = Required.DisallowNull)]        
        public string FirstName {get; set;}

        [JsonProperty("lastname", Required = Required.DisallowNull)]
        public string LastName {get; set;}
    }

    public class ClassSearchRequest : SearchRequest, IClassSearchRequest
    {       
        
        [JsonProperty("code", Required = Required.DisallowNull)]        
        public string Code {get; set;}

        [JsonProperty("title", Required = Required.DisallowNull)]
        public string Title {get; set;}

        [JsonProperty("description", Required = Required.DisallowNull)]
        public string Description {get; set;}
    }
}