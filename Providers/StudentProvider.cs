using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Core;
using Couchbase.Search;
using DemoApp1.Domain;
using DemoApp1.Util;

namespace DemoApp1.Providers
{
    public class StudentProvider: IStudentProvider
    {                           
        private static readonly string _studentKeyFormat = CBConfig.Buckets.Students.Documents.Student.StudentKeyFormat; // student::Guid
        private static readonly string _enrolledClassesDocPathFormat = CBConfig.Buckets.Students.Documents.Student.Paths.EnrolledClasses_ItemDocPathFormat;  
        private readonly IServiceWorker<IBucket, IStudent, Student, IOperationResult<Student>, IOperationResult<ICollection<Student>>, IDocumentFragment<Student>> _studentRepoWorker;        
        public StudentProvider(IServiceWorker<IBucket, IStudent, Student, IOperationResult<Student>, IOperationResult<ICollection<Student>>, IDocumentFragment<Student>> studentRepoWorker)
        {
            _studentRepoWorker = studentRepoWorker;
        }

        public async Task<IProviderResponse<IStudent>> Create(IStudent studentEntity)
        {
            studentEntity.StudentId = string.Format(_studentKeyFormat, Guid.NewGuid().ToString());            
            return await _studentRepoWorker.ProcessAsync(bucket => bucket.UpsertAsync<Student>(studentEntity.StudentId, (studentEntity as Student))); 
        }

        public async Task<IProviderResponse<IStudent>> Update(IStudentUpdateRequest updateRequest)
        {
           updateRequest.Student.ModifiedWhen = DateTime.UtcNow;
            return await _studentRepoWorker.ProcessAsync(bucket => bucket.UpsertAsync<Student>(updateRequest.Student.StudentId, (updateRequest.Student as Student), ((ulong)updateRequest.Version))); 
        }
        
        public async Task<IProviderResponse<IStudent>> Retrieve(string studentId)
        {
            return await _studentRepoWorker.ProcessAsync(bucket => bucket.GetAsync<Student>(studentId));
        }
        
        public async Task<IProviderResponse<ICollection<IStudent>>> RetrieveAll()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IProviderResponse<IStudent>> Delete(string studentId)
        {
            var response = await Retrieve(studentId);
            await _studentRepoWorker.Client.RemoveAsync(studentId);
            return response;
        }
        
        public async Task<IProviderResponse<bool>> Enroll(string classCode, string studentId)
        {
            var insertPath = string.Format(_enrolledClassesDocPathFormat, classCode);
            return await _studentRepoWorker.ProcessFragmentAsync<bool>(bucket => bucket.MutateIn<Student>(studentId)
                                                                                .Upsert(insertPath, classCode, true)
                                                                                .Upsert(CBConfig.Buckets.Students.Documents.Student.Paths.ModifiedWhen, DateTime.UtcNow, true)
                                                                                .ExecuteAsync(), insertPath);
        }
        
        public async Task<IProviderResponse<bool>> UnEnroll(string classCode, string studentId)
        {
            var deletePath = string.Format(_enrolledClassesDocPathFormat, classCode);
            return await _studentRepoWorker.ProcessFragmentAsync<bool>(bucket => bucket.MutateIn<Student>(studentId)
                                                                                .Remove(deletePath)
                                                                                .Upsert(CBConfig.Buckets.Students.Documents.Student.Paths.ModifiedWhen, DateTime.UtcNow, true)
                                                                                .ExecuteAsync(), deletePath);
        }

    }
}