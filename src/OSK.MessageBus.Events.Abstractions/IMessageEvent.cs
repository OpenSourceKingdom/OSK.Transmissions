namespace OSK.MessageBus.Events.Abstractions
{
    public interface IMessageEvent
    {
        public string TopicId { get; }
    }
}
