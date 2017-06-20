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

                _initIndexTemplates();
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

        
        private static void _initIndexTemplates()
        {                                   
            var mainIndexTemplateResponse = _elasticClient.PutIndexTemplate(ElasticConfig.IndexTemplates.Main.Name,
                                                                    p => p.Create(true) // Only add if new, don't replace
                                                                        .Template(ElasticConfig.IndexTemplates.Main.Template)
                                                                        .Order(ElasticConfig.IndexTemplates.Main.Order)
                                                                        .Mappings(ElasticConfig.IndexTemplates.Main.Mappings));
                                                                    
            var logIndexTemplateResponse = _elasticClient.PutIndexTemplate(ElasticConfig.IndexTemplates.Logs.Name,
                                                    p => p.Create(true) // Only add if new, don't replace
                                                        .Template(ElasticConfig.IndexTemplates.Logs.Template)
                                                        .Order(ElasticConfig.IndexTemplates.Logs.Order)
                                                        .Mappings(ElasticConfig.IndexTemplates.Logs.Mappings));                                                
        }
        
    }
}