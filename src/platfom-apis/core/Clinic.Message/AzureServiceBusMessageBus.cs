using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;

namespace Clinic.Message;

public class AzureServiceBusMessageBus : IMessageBus
{
    private string connectionString =
        "Endpoint=sb://youngblood.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zqAp8DvJzPVgHynD34FMI9a4clU3gcOMW+ASbHjnC+4=";
    public async Task PublishMessage(BaseMessage message, string topicName)
    {
        ISenderClient senderClient = new TopicClient(connectionString, topicName);

        var jsonMessage = JsonConvert.SerializeObject(message);
        var finalMessage = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };
        await senderClient.SendAsync(finalMessage);

        await senderClient.CloseAsync();
    }
}