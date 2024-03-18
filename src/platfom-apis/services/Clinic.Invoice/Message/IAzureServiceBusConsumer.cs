namespace Clinic.Invoice.Message
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
