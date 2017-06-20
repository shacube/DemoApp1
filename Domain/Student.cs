using System;
using Nest;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DemoApp1.Domain
{
    [ElasticsearchType(Name = "student", IdProperty = "StudentId")]
    public class Student : IStudent
    {
        public Student(DataLookupBase<string> enrolledClassCodes)
        {
            EnrolledClassCodes = enrolledClassCodes;
        }

        [Text(Name = "_id")]
        [JsonProperty("studentid")]
        public string StudentId {get; set;}

        [Text(Name = "lastname")]
        [JsonProperty("lastname", Required = Required.Always)]
        public string LastName {get; set;}

        [Text(Name = "firstname")]
        [JsonProperty("firstname", Required = Required.Always)]
        public string FirstName {get; set;}

        [Keyword(Name = "enrolledclasscodes")]
        [JsonProperty("enrolledclasscodes", Required = Required.DisallowNull)] 
        public IDataLookup<string> EnrolledClassCodes {get; set;}

        [Date(Name = "createdwhen")]
        [JsonProperty("createdwhen")]
        public DateTime CreatedWhen {get; set;}

        [Date(Name = "modifiedwhen")]
        [JsonProperty("modifiedwhen")]          
        public DateTime ModifiedWhen {get; set;} 
    }
}