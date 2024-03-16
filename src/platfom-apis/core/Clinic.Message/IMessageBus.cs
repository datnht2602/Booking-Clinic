namespace Clinic.Message;

public interface IMessageBus
{
    Task PublishMessage(BaseMessage message, string topicName);
}