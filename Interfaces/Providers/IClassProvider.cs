using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface IClassProvider
    {
        Task<IProviderResponse<IClass>> Create(IClass classEntity);
        Task<IProviderResponse<IClass>> Update(IClassUpdateRequest updateRequest);
        Task<IProviderResponse<IClass>> Retrieve(string code);
        Task<IProviderResponse<ICollection<IClass>>> RetrieveAll();
        Task<IProviderResponse<IClass>> Delete(string code);
        Task<IProviderResponse<bool>> Enroll(string classCode, string studentId);
        Task<IProviderResponse<bool>> UnEnroll(string classCode, string studentId);

    }
}