namespace OSK.MessageBus.Events.Abstractions
{
    public abstract class MessageEventBase: IMessageEvent
    {
        public string TopicId { get; set; }
    }
}
