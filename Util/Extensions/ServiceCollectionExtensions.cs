using DemoApp1;
using DemoApp1.Util;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ElasticServiceCollectionExtensions
    {
        private static IElasticConfig _elasticConfig;        
        public static IServiceCollection AddElasticOptions(this IServiceCollection services, IConfigurationSection elasticConfigSection)
        {
            _elasticConfig = new ElasticConfig();
            elasticConfigSection.Bind(_elasticConfig);

            return services.Configure<ElasticConfig>(elasticConfigSection);            
        }
        public static IServiceCollection AddElasticService(this IServiceCollection services)
        {
            ElasticClusterHelper.Initialize(_elasticConfig);
            return services.AddSingleton<IElasticClient>(ElasticClusterHelper.GetClient()); 
        }
    }
}