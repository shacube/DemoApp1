using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Couchbase;
using Couchbase.Core;
using Couchbase.IO;
using DemoApp1.Providers;
using System.Collections.Generic;

namespace DemoApp1.Util
{
    public class CBServiceWorker<TClient, TEntity, TServiceEntity, TSvcFragmentEntity>: ServiceWorkerBase<IBucket, TEntity, TServiceEntity, 
                                                                                                                  IOperationResult<TServiceEntity>, 
                                                                                                                  IOperationResult<ICollection<TServiceEntity>>,
                                                                                                                  IDocumentFragment<TSvcFragmentEntity>>
                                                                                                               where TServiceEntity : TEntity
    {
        private readonly string _bucketName;

        public CBServiceWorker(string bucketName)
            : base(ClusterHelper.GetBucket(bucketName))
        {
            _bucketName = bucketName;
        }

        protected override IProviderResponse<TEntity> MapResponse(IOperationResult<TServiceEntity> cbSvcResult)
        {
            if(cbSvcResult != null)
            {
                return new ProviderResponse<TEntity>(){
                        Data = cbSvcResult.Value,
                        Success = cbSvcResult.Success,
                        Message = cbSvcResult.Message,
                        ResponseStatus = cbSvcResult.Status.ToString("f"),
                        Exception = cbSvcResult.Exception,
                        Version = cbSvcResult.Cas
                    };
            }

            return new ProviderResponse<TEntity>(){                        
                        Success = false,
                        Message = string.Empty,
                        ResponseStatus = CustomResponseErrors.CBSvcMappingError.ToString("f")                       
                    };
        }

        protected override IProviderResponse<TEntityFragment> MapResponse<TEntityFragment>(IDocumentFragment<TSvcFragmentEntity> cbSvcResult, string fragmentKey)
        {
            if(cbSvcResult != null)
            {
                int fragmentIndex;

                dynamic data = (Int32.TryParse(fragmentKey, out fragmentIndex)) ? cbSvcResult.Content<TEntityFragment>(fragmentIndex) : cbSvcResult.Content<TEntityFragment>(fragmentKey);

                if(data == null && typeof(TEntityFragment) == typeof(bool))
                {
                    data = cbSvcResult.Exists(fragmentKey); 
                }                                                                  

                return new ProviderResponse<TEntityFragment>(){
                        Data = data,
                        Success = cbSvcResult.Success,
                        Message = cbSvcResult.Message,
                        ResponseStatus = cbSvcResult.Status.ToString("f"),
                        Exception = cbSvcResult.Exception,
                        Version = cbSvcResult.Cas
                    };
            }

            return new ProviderResponse<TEntityFragment>(){                        
                        Success = false,
                        Message = string.Empty,
                        ResponseStatus = CustomResponseErrors.CBSvcMappingError.ToString("f")                       
                    };
        }

        protected override IProviderResponse<ICollection<TEntity>> MapResponse(IOperationResult<ICollection<TServiceEntity>> operationResult)
        {            
            if(operationResult != null)
            {
                var responseEntityCollection = new List<TEntity>();
                foreach(var serviceEntity in operationResult.Value)
                {
                    responseEntityCollection.Add(serviceEntity);
                }

                var response = new ProviderResponse<ICollection<TEntity>>(){
                        Data = responseEntityCollection,
                        Success = operationResult.Success,
                        Message = operationResult.Message,
                        ResponseStatus = operationResult.Status.ToString("f"),
                        Exception = operationResult.Exception,
                        Version = operationResult.Cas
                    };                            
            }

            return new ProviderResponse<ICollection<TEntity>>(){                        
                        Success = false,
                        Message = string.Empty,
                        ResponseStatus = CustomResponseErrors.CBSvcMappingError.ToString("f")                       
                    };
        }

        protected override IProviderResponse<ICollection<TEntity>> MapResponse(ICollection<IOperationResult<TServiceEntity>> cbSvcResults)
        {
            var response = new ProviderResponse<ICollection<TEntity>>(){                        
                        Data = new List<TEntity>(),
                        Success = true,
                        Message = string.Empty,
                        ResponseStatus = Couchbase.IO.ResponseStatus.Success.ToString("f")               
                    };

            StringBuilder errorList = new StringBuilder();

            foreach(var operationResult in cbSvcResults)
            {                
                if(operationResult != null)
                {
                    if(operationResult.Success)
                    {
                        response.Data.Add(operationResult.Value);
                    }
                    else
                    {                        
                        errorList.AppendFormat(@"Message: {0}, ResponseStatus: {1}. ", operationResult.Message, operationResult.Status.ToString("f"));
                    }
                }                
            }

            if(errorList.Length > 0)
            {
                response.Success = false;
                response.Message = errorList.ToString();
                response.ResponseStatus = Couchbase.IO.ResponseStatus.InternalError.ToString("f");
            }

            return response;
        }

        public override async Task<IProviderResponse<TEntity>> ProcessAsync(Func<IBucket, Task<IOperationResult<TServiceEntity>>> operation)
        {
            IProviderResponse<TEntity> response;

            try
            {                
                var result = await operation(_client);

                if(result.ShouldRetry())
                {
                    result = await operation(_client);
                }
                
                response = MapResponse(result); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<TEntity>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = Couchbase.IO.ResponseStatus.InternalError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }

        public override async Task<IProviderResponse<TEntityFragment>> ProcessFragmentAsync<TEntityFragment>(Func<IBucket, Task<IDocumentFragment<TSvcFragmentEntity>>> operation, string fragmentKey)
        {
            IProviderResponse<TEntityFragment> response;

            try
            {                
                var result = await operation(_client);
                
                if(result.ShouldRetry())
                {
                    result = await operation(_client);
                }

                response = MapResponse<TEntityFragment>(result, fragmentKey); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<TEntityFragment>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = Couchbase.IO.ResponseStatus.InternalError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }

        public override async Task<IProviderResponse<ICollection<TEntity>>> ProcessCollectionAsync(Func<IBucket, Task<IOperationResult<ICollection<TServiceEntity>>>> operation)
        {
            IProviderResponse<ICollection<TEntity>> response;

            try
            {                
                var result = await operation(_client);

                if(result.ShouldRetry())
                {
                    result = await operation(_client);
                }
                
                response = MapResponse(result); 
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<ICollection<TEntity>>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = Couchbase.IO.ResponseStatus.InternalError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }

        public override async Task<IProviderResponse<ICollection<TEntity>>> ProcessMultipleAsync(Func<IBucket, Task<IOperationResult<TServiceEntity>[]>> operations)
        {
            IProviderResponse<ICollection<TEntity>> response;

            try
            {                
                var results = await operations(_client);                

                foreach(var result in results)
                {
                    if(result.ShouldRetry() || !result.Success)
                    {
                        //Todo
                        //Logger
                    }
                }

                response = MapResponse(results);    
            }
            catch(Exception ex)
            {
                //Todo 
                //_logger ex
                response = new ProviderResponse<ICollection<TEntity>>(){                        
                        Success = false,
                        Message = ex.Message,
                        ResponseStatus = Couchbase.IO.ResponseStatus.InternalError.ToString("f"),
                        Exception = ex
                    };
            }

            return response;
        }
    }
}