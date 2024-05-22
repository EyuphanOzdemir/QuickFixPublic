namespace MessageBus.Interface
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
