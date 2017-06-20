using System;
using System.Linq;
using Nest;
using Elasticsearch.Net;

namespace DemoApp1.Util
{
    public class ElasticClusterHelper
    {
        private static IElasticConfig _elasticConfig;
        private static IElasticClient _elasticClient {get; set;}
        public static ConnectionSettings ConnectionSettings {get; private set;}        

        private ElasticClusterHelper() { }

        public static void Initialize(IElasticConfig elasticConfig)
        {
            try
            {
                _elasticConfig = elasticConfig;

                var connectionPool = new SniffingConnectionPool(_elasticConfig.NodeUris.Select(p => new Uri(p)));            
                ConnectionSettings = new ConnectionSettings(connectionPool).DefaultIndex(ElasticConfig.Indices.Stream.Name());          
                
                _elasticClient = new ElasticClient(ConnectionSettings);  

                //_initIndexTemplates();
            }
            catch(Exception ex)
            {
                // ToDo Logger
            }
        }

        public static IElasticClient GetClient()
        {
            return _elasticClient;
        }

        /*
        private static void _initIndexTemplates()
        {                                   
                                                                       
        }
        */
    }
}