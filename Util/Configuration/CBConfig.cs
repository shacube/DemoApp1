using System;

namespace DemoApp1.Util
{
    public static class CBConfig
    {
        public static readonly string ConfigSection = "couchbase:basic";
        public struct Buckets
        {
            public struct Students
            {
                public const string BucketName = "venus";
                public struct Documents
                {
                    public struct Student
                    {
                        public const string StudentKeyFormat = @"student::{0}"; // student::Guid
                        public struct Paths
                        {
                            public const string EnrolledClasses_ItemDocPathFormat = @"enrolledclasscodes.map.{0}";                                                        
                            public const string ModifiedWhen = "modifiedwhen";
                        }
                    }
                }
            }
            
            public struct Classes
            {
                public const string BucketName = "venus";
                public struct Documents
                {
                    public struct Class
                    {
                        public const string ClassKeyFormat = @"class::{0}";    // class::{guid}, i.e. class::Guid                                                

                        public struct Paths
                        {
                            public const string EnrolledStudents_ItemDocPathFormat = @"enrolleeids.map.{0}";                                                        
                            public const string ModifiedWhen = "modifiedwhen";
                        }
                    }                    
                }
            }            
        }
    } 
}