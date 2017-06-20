using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApp1
{
    public interface IServiceWorker<TClient, TEntity, TServiceEntity, TServiceResult, TServiceCollectionResult, TServiceFragmentResult>
    {
        TClient Client {get;}
        Task<IProviderResponse<TEntity>> ProcessAsync(Func<TClient, Task<TServiceResult>> operation);
        Task<IProviderResponse<TEntityFragment>> ProcessFragmentAsync<TEntityFragment>(Func<TClient, Task<TServiceFragmentResult>> operation, string fragmentKey);        
        Task<IProviderResponse<ICollection<TEntity>>> ProcessCollectionAsync(Func<TClient, Task<TServiceCollectionResult>> operation);            
        Task<IProviderResponse<ICollection<TEntity>>> ProcessMultipleAsync(Func<TClient, Task<TServiceResult[]>> operations);                         
    }
    
}