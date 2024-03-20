using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Clinic.Message;

public class AzureServiceBusMessageBus : IMessageBus
{
    private string connectionString =
        "Endpoint=sb://youngblood.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zqAp8DvJzPVgHynD34FMI9a4clU3gcOMW+ASbHjnC+4=";
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