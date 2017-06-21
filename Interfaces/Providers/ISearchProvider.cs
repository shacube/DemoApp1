using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface ISearchProvider
    {
        Task PublishToElastic(IClass createdClass);
        Task UpdateElastic(IClass updatedClass);
        Task RemoveFromElastic(IClass removedClass);
        Task PublishToElastic(IStudent createdStudent);
        Task UpdateElastic(IStudent updatedStudent);
        Task RemoveFromElastic(IStudent removedStudent);
        Task<ISearchResponse<ICollection<IClass>>> Search(IClassSearchRequest searchParams); 
        Task<ISearchResponse<ICollection<IStudent>>> Search(IStudentSearchRequest searchParams);        
    }
}