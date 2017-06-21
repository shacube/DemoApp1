using System;

namespace DemoApp1
{
    [Flags]
    public enum Topics
    {
        Logs = 1,        
        SearchIndexes = 2,        
    }

    public enum ActivityType
    {
        ClassCreate, 
        ClassEdit,
        StudentCreate, 
        StudentEdit,
        ClassDelete,
        StudentDelete,
        StudentSearch,
        ClassSearch,
        Enroll,
        Unenroll,
        Log
    }
}