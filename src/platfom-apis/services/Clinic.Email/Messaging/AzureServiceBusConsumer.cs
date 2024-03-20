using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Message;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Mango.Services.Email.Messaging
{  
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmail;
        private readonly string checkoutMessageTopic;

        private readonly IEmailSender _emailSender;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer( IConfiguration configuration,IEmailSender emailSender)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            checkoutMessageTopic = _configuration.GetValue<string>("checkoutmessagetopic");
            _emailSender = emailSender;

            var client = new ServiceBusClient(serviceBusConnectionString);

            orderUpdatePaymentStatusProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionEmail);
        }

        public async Task Start()
        {
          
            orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            
            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

  

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            BookingDetailDto checkoutHeaderDto = JsonConvert.DeserializeObject<BookingDetailDto>(body);
            try
            {
                await _emailSender.SendEmailAsync(checkoutHeaderDto.BriefViewModel.Email, $"Đặt thành công lịch khám bác sĩ {checkoutHeaderDto.DoctorName}",
                   ReturnBodyContent(checkoutHeaderDto.BriefViewModel) );
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string ReturnBodyContent(BriefViewModel information)
        {
                return
                    $"Cảm ơn {information.UserName} đã đặt dịch vụ bên chúng tôi, thời gian đặt {information?.OrderPlacedTime}, xin vui lòng đến đúng giờ";
        }
    }
}
