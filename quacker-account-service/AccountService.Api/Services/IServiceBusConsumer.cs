namespace AccountService.Api.Services
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
    }
}