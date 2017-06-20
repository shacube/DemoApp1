using System.Threading.Tasks;

namespace DemoApp1
{
    public interface IFirehoseProvider
    {
        Task PublishAsync<T>(Topics topics, IActivity<T> activity);     
    }
}