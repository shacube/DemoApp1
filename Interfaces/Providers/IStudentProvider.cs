using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface IStudentProvider
    {
        Task<IProviderResponse<IStudent>> Create(IStudent studentEntity);
        Task<IProviderResponse<IStudent>> Update(IStudentUpdateRequest updateRequest);
        Task<IProviderResponse<IStudent>> Retrieve(string studentId);
        Task<IProviderResponse<ICollection<IStudent>>> RetrieveAll();
        Task<IProviderResponse<IStudent>> Delete(string studentId);
        Task<IProviderResponse<bool>> Enroll(string classCode, string studentId);
        Task<IProviderResponse<bool>> UnEnroll(string classCode, string studentId);

    }
}