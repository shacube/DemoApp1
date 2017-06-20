using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Core;
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
        {}

        public async Task<IProviderResponse<IStudent>> Update(IStudent studentEntity, ulong version)
        {}
        
        public async Task<IProviderResponse<IStudent>> Retrieve(string studentId)
        {}
        
        public async Task<IProviderResponse<ICollection<IStudent>>> RetrieveAll()
        {}
        
        public async Task<IProviderResponse<IStudent>> Delete(string studentId)
        {}
        
        public async Task<IProviderResponse> Enroll(string classCode, string studentId)
        {}
        
        public async Task<IProviderResponse> UnEnroll(string classCode, string studentId)
        {}

    }
}