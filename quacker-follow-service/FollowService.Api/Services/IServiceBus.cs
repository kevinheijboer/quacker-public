using System.Threading.Tasks;

namespace FollowService.Api.Services
{
    public interface IServiceBus
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string topicName);
    }
}