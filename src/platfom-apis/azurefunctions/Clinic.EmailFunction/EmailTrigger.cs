using System;
using Clinic.DTO.Models.Message;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clinic.EmailFunction
{
    public class EmailTrigger
    {
        private readonly ILogger<EmailTrigger> _logger;

        public EmailTrigger(ILogger<EmailTrigger> log)
        {
            _logger = log;
        }

        [FunctionName("EmailTrigger")]
        public void Run([ServiceBusTrigger("checkoutmessagetopic", "emailSubscription", Connection = "MySBConnection")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            BookingDetailDto checkoutHeaderDto = JsonConvert.DeserializeObject<BookingDetailDto>(mySbMsg);
        }
    }
}
