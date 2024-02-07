using System.Threading.Tasks;

namespace AuthService.Api.Services
{
    public interface IServiceBus
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string topicName);
    }
}