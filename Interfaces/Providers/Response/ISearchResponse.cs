using System.Threading.Tasks;

namespace DemoApp1
{
    public interface ISearchResponse<T> : IProviderResponse<T>
    {
        int Offset {get; set;}
        int PageSize {get; set;}
    } 
}