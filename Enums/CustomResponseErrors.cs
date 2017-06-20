9namespace DemoApp1
{
    public enum CustomResponseErrors
    {
        StudentRetrieveFailed = 0,
        StudentCreateFailed = 1,
        StudentUpdateFailed = 2,
        ClassRetrieveFailed = 3,
        ClassCreateFailed = 4,
        ClassUpdateFailed = 5,
        CBSvcMappingError = 6,        
        ElasticSvcMappingError = 7,
        SearchFailed = 8,
        WithErrors = 9
    }
}