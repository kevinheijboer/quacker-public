namespace FollowService.Api.Services
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
    }
}