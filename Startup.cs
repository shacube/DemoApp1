using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Couchbase;
using Couchbase.Core;
using Couchbase.Configuration.Client;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using IBucket = Couchbase.Core.IBucket;
using DemoApp1.Domain;
using DemoApp1.Providers;
//using DemoApp1.Processors;
using DemoApp1.Util;

namespace DemoApp1
{
    public class Startup
    {   
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var definition = new CouchbaseClientDefinition();
            Configuration.GetSection("couchbase:basic").Bind(definition);
            var clientConfig = new ClientConfiguration(definition);
            ClusterHelper.Initialize(clientConfig);

            /*
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);
            */
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.            
            services.AddMvc();    

            // Configuration
            //services.AddDefaultAWSOptions(Configuration.GetAWSOptions());              
            services.AddElasticOptions(Configuration.GetSection(ElasticConfig.ConfigSection));            
            
            // Providers
            //services.AddAWSService<IAmazonS3>();
            services.AddSingleton<IClassProvider, ClassProvider>();
            services.AddSingleton<IStudentProvider, StudentProvider>();
            //services.AddSingleton<IFirehoseProvider, FirehoseProvider>();
            services.AddSingleton<ISearchProvider, SearchProvider>();            
            //services.AddSingleton<IMsgQueueProvider<Activity<T>>, MsgQueueProvider>();            
            services.AddElasticService();                        

            // Processors
            //services.AddSingleton<SearchProcessor>();            
            //services.AddSingleton<LogsProcessor>();        

            // Service workers/mappers
            // CB workers
            services.AddSingleton<IServiceWorker<IBucket, IStudent, Domain.Student, IOperationResult<Domain.Student>, IOperationResult<ICollection<Domain.Student>>, IDocumentFragment<Domain.Student>>>
                    (new CBServiceWorker<IBucket, IStudent, Domain.Student, Domain.Student>(CBConfig.Buckets.Students.BucketName));
            services.AddSingleton<IServiceWorker<IBucket, IClass, Domain.Class, IOperationResult<Domain.Class>, IOperationResult<ICollection<Domain.Class>>, IDocumentFragment<Domain.Class>>>
                    (new CBServiceWorker<IBucket, IClass, Domain.Class, Domain.Class>(CBConfig.Buckets.Classes.BucketName));
            
            // Elastic index worker.
            //services.AddSingleton<IServiceWorker<IElasticClient, object, object, IResponse, IBulkResponse, IBulkResponse>, ElasticIndexWorker<IElasticClient, object, object, IResponse, IBulkResponse, IBulkResponse>>();
            
            // Validators
            services.AddSingleton<ISchemaValidator<Student>, SchemaValidator<Student>>();            
            services.AddSingleton<ISchemaValidator<Class>, SchemaValidator<Class>>();
            services.AddSingleton<ISchemaValidator<StudentUpdateRequest>, SchemaValidator<StudentUpdateRequest>>();
            services.AddSingleton<ISchemaValidator<ClassUpdateRequest>, SchemaValidator<ClassUpdateRequest>>();
            services.AddSingleton<ISchemaValidator<StudentSearchRequest>, SchemaValidator<StudentSearchRequest>>();            
            services.AddSingleton<ISchemaValidator<ClassSearchRequest>, SchemaValidator<ClassSearchRequest>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
