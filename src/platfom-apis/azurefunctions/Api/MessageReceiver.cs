using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Api
{
    public class MessageReceiver
    {
        private readonly ILogger<MessageReceiver> _logger;

        public MessageReceiver(ILogger<MessageReceiver> logger)
        {
            _logger = logger;
        }

        [Function("MessageReceiver")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // Read the connection string from configurations
            string connectionstring = "Endpoint=sb://youngblood.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zqAp8DvJzPVgHynD34FMI9a4clU3gcOMW+ASbHjnC+4=";

            // Initialize Service bus connection 
            await using ServiceBusClient serviceBusClient = new(connectionstring);

            // Initialize a sender object with queue name
            var sender = serviceBusClient.CreateSender("ordermessagetopic");

            // Create message for service bus 
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(requestBody))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await serviceBusClient.DisposeAsync();

            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
