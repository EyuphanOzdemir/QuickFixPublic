namespace MessageBus.Interface
{
    public interface IMessageBusPublisher
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }
}
