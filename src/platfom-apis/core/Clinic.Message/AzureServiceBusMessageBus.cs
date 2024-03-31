using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Clinic.Message;

public class AzureServiceBusMessageBus : IMessageBus
{
    private string connectionString =
        "Endpoint=sb://clinicsp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QxSugQSghx2aVDBESG6sx6aUdwANHx7Cx+ASbMqsYS0=";
    public async Task PublishMessage(BaseMessage message, string topicName)
    {
        await using var client = new ServiceBusClient(connectionString);

        ServiceBusSender sender = client.CreateSender(topicName);

        var jsonMessage = JsonConvert.SerializeObject(message);
        ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };

        await sender.SendMessageAsync(finalMessage);

        await client.DisposeAsync();
    }
}