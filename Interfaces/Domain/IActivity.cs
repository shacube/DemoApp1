using System;

namespace DemoApp1
{
    public interface IActivity<T>
    {
        string Id {get; set;}
        ActivityType ActivityType {get; set;}        
        DateTime Timestamp {get; set;}    
        IActivityPayload<T> Payload {get; set;}
    }

    public interface IActivityPayload<T>
    {
        T Data {get; set;}
    }
}