using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Clinic.Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clinic.OrderMessage;

public static class MessageReceiver
{
    [FunctionName("MessageReceiver")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        // Initialize Service bus connection 
        await using ServiceBusClient serviceBusClient = new("Endpoint=sb://clinicsp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QxSugQSghx2aVDBESG6sx6aUdwANHx7Cx+ASbMqsYS0=");

        // Initialize a sender object with queue name
        var sender = serviceBusClient.CreateSender("ordermessagetopic");

        // Create message for service bus 
        ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(requestBody))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };

        await sender.SendMessageAsync(finalMessage);

        await serviceBusClient.DisposeAsync();
        return new OkResult();

    }
}