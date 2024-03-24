namespace Clinic.Booking.Message
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
