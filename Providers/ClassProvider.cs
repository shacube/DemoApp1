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
        private static readonly string _classKeyFormat = CBConfig.Buckets.Classes.Documents.Class.ClassKeyFormat; // class::Guid
        private static readonly string _enrolledStudentsDocPathFormat = CBConfig.Buckets.Classes.Documents.Class.Paths.EnrolledStudents_ItemDocPathFormat;        
        private readonly IServiceWorker<IBucket, IClass, Class, IOperationResult<Class>, IOperationResult<ICollection<Class>>, IDocumentFragment<Class>> _classRepoWorker;        
        public ClassProvider(IServiceWorker<IBucket, IClass, Class, IOperationResult<Class>, IOperationResult<ICollection<Class>>, IDocumentFragment<Class>> classRepoWorker)
        {
            _classRepoWorker = classRepoWorker;
        }

        public async Task<IProviderResponse<IClass>> Create(IClass classEntity)
        {}

        public async Task<IProviderResponse<IClass>> Update(IClass classEntity, ulong version)
        {}

        public async Task<IProviderResponse<IClass>> Retrieve(string code)
        {}

        public async Task<IProviderResponse<ICollection<IClass>>> RetrieveAll()
        {}

        public async Task<IProviderResponse<IClass>> Delete(string code)
        {}

        public async Task<IProviderResponse> Enroll(string classCode, string studentId)
        {}

        public async Task<IProviderResponse> UnEnroll(string classCode, string studentId)
        {}

    }
}