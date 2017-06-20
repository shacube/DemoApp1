using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface IClassProvider
    {
        Task<IProviderResponse<IClass>> Create(IClass classEntity);
        Task<IProviderResponse<IClass>> Update(IClass classEntity, ulong version);
        Task<IProviderResponse<IClass>> Retrieve(string code);
        Task<IProviderResponse<ICollection<IClass>>> RetrieveAll();
        Task<IProviderResponse<IClass>> Delete(string code);
        Task<IProviderResponse> Enroll(string classCode, string studentId);
        Task<IProviderResponse> UnEnroll(string classCode, string studentId);

    }
}