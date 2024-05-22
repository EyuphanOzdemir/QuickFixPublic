using Azure.Messaging.ServiceBus;
using MessageBus.Interface;
using Newtonsoft.Json;
using System.Text;

namespace MessageBus.Implementation
{
    public class MessageBusPublisher : IMessageBusPublisher
    {

        private string connectionString = "<Azure Service Bus Connection string>";

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding
                .UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
