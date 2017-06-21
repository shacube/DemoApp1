using Newtonsoft.Json;
using DemoApp1.Domain;

namespace DemoApp1.Providers
{
    public class ClassUpdateRequest : IClassUpdateRequest
    {

        [JsonConstructor]
        public ClassUpdateRequest(Class classObj)
        {
            ClassObj = classObj;
        }

        [JsonProperty("classobj", Required = Required.Always)]
        public IClass ClassObj {get; set;}

        [JsonProperty("version", Required = Required.Always)]
        public long Version {get; set;}
    }

    public class StudentUpdateRequest : IStudentUpdateRequest
    {

        [JsonConstructor]
        public StudentUpdateRequest(Student student)
        {
            Student = student;
        }

        [JsonProperty("student", Required = Required.Always)]
        public IStudent Student {get; set;}

        [JsonProperty("version", Required = Required.Always)]
        public long Version {get; set;}
    }
}