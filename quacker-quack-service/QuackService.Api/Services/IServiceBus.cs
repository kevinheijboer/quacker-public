using System.Threading.Tasks;

namespace QuackService.Api.Services
{
    public interface IServiceBus
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string topicName);
    }
}