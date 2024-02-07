namespace TimelineService.Api.Services
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
    }
}