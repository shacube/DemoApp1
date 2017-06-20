using System;

namespace DemoApp1
{
    public interface IProviderResponse
    {        
        bool Success {get;}
        string ResponseStatus {get;}
        string Message {get;}
        Exception Exception {get;}
        ulong Version {get;}
    }

    public interface IProviderResponse<T> : IProviderResponse
    {
        T Data {get;}
    }
}