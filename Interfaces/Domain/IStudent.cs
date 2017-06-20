using System;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface IStudent
    {
        string StudentId {get; set;}
        string LastName {get; set;}
        string FirstName {get; set;}
        IDataLookup<string> EnrolledClassCodes {get; set;}
        DateTime CreatedWhen {get; set;}
        DateTime ModifiedWhen {get; set;} 
    }
}