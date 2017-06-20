using System;
using Nest;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DemoApp1.Domain
{
    [ElasticsearchType(Name = "class", IdProperty = "Code")]
    public class Class : IClass
    {
        [JsonConstructor]
        public Class(DataLookupBase<string> enrolleeIds)
        {
            EnrolleeIds = enrolleeIds;
        }

        [Text(Name = "_id")]
        [JsonProperty("code")]
        public string Code {get; set;}
        
        [Text(Name = "title")]
        [JsonProperty("title", Required = Required.Always)]
        public string Title {get; set;}

        [Text(Name = "description")]
        [JsonProperty("description", Required = Required.Always)]
        public string Description {get; set;}

        [Keyword(Name = "enrolleeids")]
        [JsonProperty("enrolleeids", Required = Required.DisallowNull)] 
        public IDataLookup<string> EnrolleeIds {get; set;}

        [Date(Name = "createdwhen")]
        [JsonProperty("createdwhen")]
        public DateTime CreatedWhen {get; set;}

        [Date(Name = "modifiedwhen")]
        [JsonProperty("modifiedwhen")]          
        public DateTime ModifiedWhen {get; set;} 
    }
}