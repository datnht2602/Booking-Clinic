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


        private readonly IInvoiceService repository;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(IInvoiceService repository, IConfiguration configuration)
        {
            this.repository = repository;
            _configuration = configuration;
            
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            checkOutProcessor = client.CreateProcessor(checkoutMessageTopic,subscriptionCheckOut);
        }

        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            checkOutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkOutProcessor.StartProcessingAsync();
            
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
                OrderTotal = checkoutHeaderDto.OrderTotal,
                Coupon = checkoutHeaderDto.CouponCode
            };


            await repository.AddInvoiceAsync(orderHeader);
            await args.CompleteMessageAsync(args.Message);

        }
        

    }
}
