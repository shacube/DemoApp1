using System;
using Newtonsoft.Json;
using Nest;

namespace DemoApp1.Domain
{
    public class Activity<T> : IActivity<T>
    {
        public Activity(){}

        [Text(Name = "_id")]
        [JsonProperty("id")]
        public string Id {get; set;}

        [Keyword(Name = "activitytype")]
        [JsonProperty("activitytype", Required = Required.Always)]
        public ActivityType ActivityType {get; set;}
        
        [Date(Name = "timestamp")]
        [JsonProperty("timestamp", Required = Required.Always)]
        public DateTime Timestamp {get; set;}                

        [Nested(Name = "payload")]
        [JsonProperty("payload", Required = Required.Always)]
        public IActivityPayload<T> Payload {get; set;}
    }

    public class ActivityPayload<T> :  IActivityPayload<T>
    {
        [JsonProperty("data", Required = Required.Always)]
        public T Data {get; set;}
    }
}