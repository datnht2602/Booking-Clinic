using Azure.Messaging.ServiceBus;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Message;
using Clinic.Message;
using Newtonsoft.Json;
using System.Text;
using AutoMapper;
using Clinic.Booking.Contracts;
using Clinic.DTO.Models.Model;


namespace Clinic.Booking.Message
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionOrder;
        private readonly string orderMessageTopic;
        private readonly IMapper autoMapper;
        private readonly IMessageBus messageBus;
        private readonly IBookingService repository;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(IBookingService repository, IConfiguration configuration,IMapper autoMapper, IMessageBus messageBus)
        {
            this.repository = repository;
            _configuration = configuration;
            
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionOrder = _configuration.GetValue<string>("SubscriptionOrder");
            orderMessageTopic = _configuration.GetValue<string>("OrderMessageTopic");
            this.autoMapper = autoMapper;
            this.messageBus = messageBus;
            var client = new ServiceBusClient(serviceBusConnectionString);

            checkOutProcessor = client.CreateProcessor(orderMessageTopic,subscriptionOrder);
        }

        public async Task Start()
        {
            checkOutProcessor.ProcessMessageAsync += OnOrderMessageReceived;
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

        private async Task OnOrderMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            InvoiceMessage orderMessage = JsonConvert.DeserializeObject<InvoiceMessage>(body);
            try
            {
                var getExistingBooking = await repository
                    .GetBookingAsync(
                        $" e.id= '{orderMessage.Id}'")
                    .ConfigureAwait(false);
                BookingDetailsViewModel? existingBooking = getExistingBooking.FirstOrDefault();
                if (existingBooking != null)
                {
                    existingBooking.InvoiceId = orderMessage.InvoiceId;
                    existingBooking.OrderStatus = OrderStatus.Submitted.ToString();
                    
                    var result = await repository.UpdateBookingAsync(existingBooking);
                    if (result.IsSuccessStatusCode)
                    {
                        UpdateSchedule item = new()
                        {
                            DoctorId = existingBooking.DoctorId,
                            OrderTime = existingBooking.OrderPlacedDate,
                            Detail =  existingBooking.BriefViewModel,
                            UserId = existingBooking.UserId
                        };
                        var userResponse = await repository.UpdateSchedule(item);
                        if (userResponse.IsSuccessStatusCode)
                        {
                            var bookingDto = this.autoMapper.Map<BookingDetailDto>(existingBooking);
                            bookingDto.InvoiceId = orderMessage.InvoiceId;
                            await messageBus.PublishMessage(bookingDto, "checkoutmessagetopic");
                        } 
                    }
                }
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
