using System;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApp1.Util;
using DemoApp1.Domain;
using Newtonsoft.Json;

namespace DemoApp1.Providers
{
    public class SearchProvider : ISearchProvider
    {
        private readonly IElasticClient _elasticProvider;
        public SearchProvider(IElasticClient elasticProvider)
        {
            _elasticProvider = elasticProvider;
        }
        public async Task PublishToElastic(IClass createdClass)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var createReq = new BulkIndexOperation<Class>(createdClass as Class);
            createReq.Index = ElasticConfig.Indices.Main.Name();
            bulkOps.Add(createReq);

            var createReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Create(), Created Class - {JsonConvert.SerializeObject(createdClass)}")},
                                                                                          ActivityType = ActivityType.ClassCreate});
            createReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(createReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps } );
        }

        public async Task UpdateElastic(IClass updatedClass)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var updateReq = new BulkUpdateOperation<Class, object>(updatedClass.Code);
            updateReq.Index = ElasticConfig.Indices.Main.Name();
            updateReq.DocAsUpsert = true;
            updateReq.Doc = updatedClass;
            bulkOps.Add(updateReq);

            var updateReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Edit(), Edited Class - {JsonConvert.SerializeObject(updatedClass)}")},
                                                                                          ActivityType = ActivityType.ClassEdit});
            updateReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(updateReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps } );
        }

        public async Task RemoveFromElastic(IClass removedClass)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var updateReq = new BulkDeleteOperation<Class>(removedClass.Code);
            updateReq.Index = ElasticConfig.Indices.Main.Name();
            bulkOps.Add(updateReq);

            var updateReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Delete(), Deleted Class - {JsonConvert.SerializeObject(removedClass)}")},
                                                                                          ActivityType = ActivityType.ClassDelete});
            updateReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(updateReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps } );        
        }

        public async Task PublishToElastic(IStudent createdStudent)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var createReq = new BulkIndexOperation<Student>(createdStudent as Student);
            createReq.Index = ElasticConfig.Indices.Main.Name();
            bulkOps.Add(createReq);

            var createReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Create(), Created Student - {JsonConvert.SerializeObject(createdStudent)}")},
                                                                                          ActivityType = ActivityType.Log});
            createReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(createReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps } );
        }

        public async Task UpdateElastic(IStudent updatedStudent)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var updateReq = new BulkUpdateOperation<Student, object>(updatedStudent.StudentId);
            updateReq.Index = ElasticConfig.Indices.Main.Name();
            updateReq.DocAsUpsert = true;
            updateReq.Doc = updatedStudent;
            bulkOps.Add(updateReq);

            var updateReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Edit(), Edited Student - {JsonConvert.SerializeObject(updatedStudent)}")},
                                                                                          ActivityType = ActivityType.StudentEdit});
            updateReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(updateReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps } );
        }

        public async Task RemoveFromElastic(IStudent removedStudent)
        {
            List<IBulkOperation> bulkOps = new List<IBulkOperation>();
            
            var updateReq = new BulkDeleteOperation<Class>(removedStudent.StudentId);
            updateReq.Index = ElasticConfig.Indices.Main.Name();
            bulkOps.Add(updateReq);

            var updateReqLog = new BulkIndexOperation<Activity<string>>(new Activity<string>(){ Id = Guid.NewGuid().ToString(),
                                                                                          Timestamp = DateTime.UtcNow,
                                                                                          Payload = new ActivityPayload<string>()
                                                                                                    { Data = string.Format($"SearchProvider: Delete(), Deleted Student - {JsonConvert.SerializeObject(removedStudent)}")},
                                                                                          ActivityType = ActivityType.StudentDelete});
            updateReqLog.Index = ElasticConfig.Indices.Logs.Name();
            bulkOps.Add(updateReqLog);

            await _elasticProvider.BulkAsync(new BulkRequest() { Operations = bulkOps });        
        }

        public async Task<ISearchResponse<ICollection<IClass>>> Search(IClassSearchRequest searchParams)
        {
            throw new NotImplementedException();
        }
        public async Task<ISearchResponse<ICollection<IStudent>>> Search(IStudentSearchRequest searchParams)
        {
            throw new NotImplementedException();
        }
    }
}