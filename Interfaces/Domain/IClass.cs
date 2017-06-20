using System;
using System.Collections.Generic;

namespace DemoApp1
{    
    public interface IClass
    {
        string Code {get; set;}
        string Title {get; set;}
        string Description {get; set;}
        IDataLookup<string> EnrolleeIds {get; set;}
        DateTime CreatedWhen {get; set;}
        DateTime ModifiedWhen {get; set;} 
    }
}