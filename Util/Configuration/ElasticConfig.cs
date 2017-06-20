using System;
using Nest;

namespace DemoApp1.Util
{
    public class ElasticConfig : IElasticConfig
    {
        public static readonly string ConfigSection = "elastic";
        public struct Indices
        {            
            public struct Main
            {
                public static string Name() {  return string.Format($"demoapp1-main-{DateTime.UtcNow.ToString("yy")}"); }
            }

            public struct Logs
            {
                public static string Name() { return string.Format($"demoapp1-logs-{DateTime.UtcNow.ToString("MMyy")}"); }                
            }

        }

        public struct IndexTemplates
        {
            public struct Main
            {
                public static readonly string Name = "demoapp1_main_mappings";
                public static readonly int Order = 2;
                public static readonly string Template = "demoapp1-main-*";
                public static Func<MappingsDescriptor, IPromise<IMappings>> Mappings = (mp) => mp.Map<Domain.Student>(m => m.AutoMap()                                                                                        
                                                                                                                            .Properties(props => props
                                                                                                                                    .Nested<Domain.DataLookupBase<string>>(lo => lo.Name(loc => loc.EnrolledClassCodes)
                                                                                                                                                                .AutoMap()
                                                                                                                                    )                                                                                                                                    
                                                                                                                            )    
                                                                                                ).Map<Domain.Class>(m => m.AutoMap()                                                                                        
                                                                                                                            .Properties(props => props
                                                                                                                                    .Nested<Domain.DataLookupBase<string>>(lo => lo.Name(loc => loc.EnrolleeIds)
                                                                                                                                                                .AutoMap()
                                                                                                                                    )                                                                                                                                    
                                                                                                                            )    
                                                                                                );
            }

            public struct Logs
            {
                public static readonly string Name = "demoapp1_logs_mappings";
                public static readonly int Order = 2;
                public static readonly string Template = "demoapp1-logs-*";
                public static Func<MappingsDescriptor, IPromise<IMappings>> Mappings = (mp) => mp.Map<Domain.Activity<string>>(m => m.Properties(props => props                                                                                                                                            
                                                                                                                                            .Date(pc => pc.Name(oc => oc.Timestamp))
                                                                                                                                            .Nested<Domain.ActivityPayload<string>>(nst => nst.Name(o => o.Payload)
                                                                                                                                                                .AutoMap()
                                                                                                                                            )
                                                                                                                                    )    
                                                                                                                                );
            }
        }
        public string[] NodeUris { get; set; }
        public int IndexBatchSize { get; set; }
    }
}