using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Core;
using DemoApp1.Domain;
using DemoApp1.Util;

namespace DemoApp1.Providers
{
    public class ClassProvider: IClassProvider
    {                           
        private static readonly string _enrolledStudentsDocPathFormat = CBConfig.Buckets.Classes.Documents.Class.Paths.EnrolledStudents_ItemDocPathFormat;        
        private readonly IServiceWorker<IBucket, IClass, Class, IOperationResult<Class>, IOperationResult<ICollection<Class>>, IDocumentFragment<Class>> _classRepoWorker;        
        public ClassProvider(IServiceWorker<IBucket, IClass, Class, IOperationResult<Class>, IOperationResult<ICollection<Class>>, IDocumentFragment<Class>> classRepoWorker)
        {
            _classRepoWorker = classRepoWorker;
        }

        public async Task<IProviderResponse<IClass>> Create(IClass classEntity)
        {                       
            return await _classRepoWorker.ProcessAsync(bucket => bucket.UpsertAsync<Class>(classEntity.Code, (classEntity as Class)));   
        }

        public async Task<IProviderResponse<IClass>> Update(IClassUpdateRequest updateRequest)
        {
            updateRequest.ClassObj.ModifiedWhen = DateTime.UtcNow;
            return await _classRepoWorker.ProcessAsync(bucket => bucket.UpsertAsync<Class>(updateRequest.ClassObj.Code, (updateRequest.ClassObj as Class), ((ulong)updateRequest.Version)));
        }

        public async Task<IProviderResponse<IClass>> Retrieve(string code)
        {
            return await _classRepoWorker.ProcessAsync(bucket => bucket.GetAsync<Class>(code));
        }

        public async Task<IProviderResponse<ICollection<IClass>>> RetrieveAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IProviderResponse<IClass>> Delete(string code)
        {
            var response = await Retrieve(code);
            await _classRepoWorker.Client.RemoveAsync(code);
            return response;
        }

        public async Task<IProviderResponse<bool>> Enroll(string classCode, string studentId)
        {
            var insertPath = string.Format(_enrolledStudentsDocPathFormat, studentId);
            return await _classRepoWorker.ProcessFragmentAsync<bool>(bucket => bucket.MutateIn<Class>(classCode)
                                                                                .Upsert(insertPath, studentId, true)
                                                                                .Upsert(CBConfig.Buckets.Classes.Documents.Class.Paths.ModifiedWhen, DateTime.UtcNow, true)
                                                                                .ExecuteAsync(), insertPath);
        }

        public async Task<IProviderResponse<bool>> UnEnroll(string classCode, string studentId)
        {
            var deletePath = string.Format(_enrolledStudentsDocPathFormat, studentId);
            return await _classRepoWorker.ProcessFragmentAsync<bool>(bucket => bucket.MutateIn<Class>(classCode)
                                                                                .Remove(deletePath)
                                                                                .Upsert(CBConfig.Buckets.Classes.Documents.Class.Paths.ModifiedWhen, DateTime.UtcNow, true)
                                                                                .ExecuteAsync(), deletePath);
        }

    }
}