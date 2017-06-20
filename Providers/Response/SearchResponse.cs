namespace DemoApp1.Providers
{
    public class SearchResponse<T> : ProviderResponse<T>, ISearchResponse<T>
    {
        public int Offset {get; set;}
        public int PageSize {get; set;}
    }

}