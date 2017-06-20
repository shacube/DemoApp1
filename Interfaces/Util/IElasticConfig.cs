using System.Collections.Generic;

namespace DemoApp1
{
    public interface IElasticConfig
    {
        string[] NodeUris {get; set;}
        int IndexBatchSize { get; set; }
    }
}