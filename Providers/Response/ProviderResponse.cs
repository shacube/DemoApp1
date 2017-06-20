using System;

namespace DemoApp1.Providers
{
    public class ProviderResponse: IProviderResponse
    {        
        public bool Success {get; set;}
        public string ResponseStatus {get; set;}
        public string Message {get; set; }
        public Exception Exception {get; set; }
        public ulong Version {get; set;}
    }

    public class ProviderResponse<T>: ProviderResponse, IProviderResponse<T>
    {
        public T Data {get; set;}
    }
}