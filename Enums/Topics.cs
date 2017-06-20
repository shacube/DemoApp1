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
        ClassCreateEdit,
        StudentCreateEdit,
        ClassDelete,
        StudentDelete,
        StudentSearch,
        ClassSearch,
        Log
    }
}