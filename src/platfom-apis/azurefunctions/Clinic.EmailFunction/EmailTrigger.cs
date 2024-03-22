using System;
using Clinic.DTO.Models.Message;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Clinic.DTO.Models;

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
        public async Task Run([ServiceBusTrigger("checkoutmessagetopic", "emailSubscription", Connection = "MySBConnection")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            BookingDetailDto checkoutHeaderDto = JsonConvert.DeserializeObject<BookingDetailDto>(mySbMsg);
            string mailTo = checkoutHeaderDto.BriefViewModel.Email;
            string mailFrom = "datnht2602@gmail.com";
            string topic = $"Đặt thành công lịch khám bác sĩ {checkoutHeaderDto.DoctorName}";
            string content = ReturnBodyContent(checkoutHeaderDto.BriefViewModel);

            string smtpaccount = "datpron@gmail.com";
            string smtppassword = "dusdbwywthifggim";
            await MailUtils.SendMailGoogleSmtp(mailFrom, mailTo, topic,content, smtpaccount, smtppassword);
        }
        private string ReturnBodyContent(BriefViewModel information)
        {
            return
                $"Cảm ơn {information.UserName} đã đặt dịch vụ bên chúng tôi, thời gian đặt {information?.OrderPlacedTime}, xin vui lòng đến đúng giờ";
        }
    }
}
