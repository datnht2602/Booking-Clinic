using Azure.Messaging.ServiceBus;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Message;
using Clinic.Invoice.Contracts;
using Clinic.Invoice.Services;
using Clinic.Message;
using Newtonsoft.Json;
using System.Text;


namespace Clinic.Invoice.Message
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckOut;
        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentProcessTopic;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly IInvoiceService repository;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        public AzureServiceBusConsumer(IInvoiceService repository, IConfiguration configuration, IMessageBus messageBus)
        {
            this.repository = repository;
            _configuration = configuration;
            _messageBus = messageBus;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopics");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            checkOutProcessor = client.CreateProcessor(checkoutMessageTopic,subscriptionCheckOut);
           /* orderUpdatePaymentStatusProcessor =
                client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionCheckOut);*/
        }

        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkOutProcessor.StartProcessingAsync();

            //orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            /*orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();*/
        }

        public async Task Stop()
        {
            await checkOutProcessor.StopProcessingAsync();
            await checkOutProcessor.DisposeAsync();

            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            BookingDetailDto checkoutHeaderDto = JsonConvert.DeserializeObject<BookingDetailDto>(body);

            InvoiceDetailsViewModel orderHeader = new()
            {
                OrderId = checkoutHeaderDto.BookingId,
                BriefViewModel = checkoutHeaderDto.BriefViewModel,
                PaymentMode = checkoutHeaderDto.PaymentMode,
                Products = checkoutHeaderDto.Products,
            };


            await repository.AddInvoiceAsync(orderHeader);


            /*            PaymentRequestMessage paymentRequestMessage = new()
                        {
                            Name = orderHeader.FirstName + " " + orderHeader.LastName,
                            CardNumber = orderHeader.CardNumber,
                            CVV = orderHeader.CVV,
                            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                            OrderId = orderHeader.OrderHeaderId,
                            OrderTotal = orderHeader.OrderTotal,
                            Email = orderHeader.Email
                        };*/

            /* try
             {
                 await _messageBus.PublishMessage(paymentRequestMessage, orderPaymentProcessTopic);
                 await args.CompleteMessageAsync(args.Message);
             }
             catch (Exception e)
             {
                 throw;
             }*/

        }

        /* private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
         {
             var message = args.Message;
             var body = Encoding.UTF8.GetString(message.Body);

             UpdatePaymentResultMessage paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

             await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);
             await args.CompleteMessageAsync(args.Message);

         }*/

    }
}
