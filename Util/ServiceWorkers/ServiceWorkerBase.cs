using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nest;
using DemoApp1.Providers;

namespace DemoApp1.Util
{
    public abstract class ServiceWorkerBase<TClient, TEntity, TServiceEntity, TResult, TCollectionResult, TFragmentResult> : IServiceWorker<TClient, TEntity, TServiceEntity, TResult, TCollectionResult, TFragmentResult>
    {
        protected readonly TClient _client;
        public virtual TClient Client { get { return _client; } }
        public ServiceWorkerBase(TClient client)
        {
            _client = client;
        }        
        
        protected abstract IProviderResponse<TEntity> MapResponse(TResult svcResult);
        protected abstract IProviderResponse<TEntityFragment> MapResponse<TEntityFragment>(TFragmentResult svcFragmentResult, string fragmentKey);        
        protected abstract IProviderResponse<ICollection<TEntity>> MapResponse(TCollectionResult bulkOpResult);        
        protected abstract IProviderResponse<ICollection<TEntity>> MapResponse(ICollection<TResult> svcResults);

        public virtual async Task<IProviderResponse<TEntity>> ProcessAsync(Func<TClient, Task<TResult>> operation)
        {
            IProviderResponse<TEntity> response;

            try
            {                
                var result = await operation(_client);                                                                
                response = MapResponse(result); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<TEntity>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = HttpStatusCode.InternalServerError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }

        public virtual async Task<IProviderResponse<TEntityFragment>> ProcessFragmentAsync<TEntityFragment>(Func<TClient, Task<TFragmentResult>> operation, string fragmentKey)
        {
            IProviderResponse<TEntityFragment> response;

            try
            {                
                var result = await operation(_client);                                                                
                response = MapResponse<TEntityFragment>(result, fragmentKey); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<TEntityFragment>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = HttpStatusCode.InternalServerError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }

        public virtual async Task<IProviderResponse<ICollection<TEntity>>> ProcessCollectionAsync(Func<TClient, Task<TCollectionResult>> operation)
        {
            IProviderResponse<ICollection<TEntity>> response;

            try
            {                
                var result = await operation(_client);                                
                response = MapResponse(result); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<ICollection<TEntity>>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = HttpStatusCode.InternalServerError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }   
        public virtual async Task<IProviderResponse<ICollection<TEntity>>> ProcessMultipleAsync(Func<TClient, Task<TResult[]>> operations)
        {
            IProviderResponse<ICollection<TEntity>> response;

            try
            {                
                var results = await operations(_client);                
                response = MapResponse(results);    
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<ICollection<TEntity>>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = HttpStatusCode.InternalServerError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }
    }
}

