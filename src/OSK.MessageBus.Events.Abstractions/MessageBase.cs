namespace OSK.MessageBus.Events.Abstractions
{
    public abstract class MessageBase: IMessage
    {
        public string TopicId { get; set; }
    }
}
