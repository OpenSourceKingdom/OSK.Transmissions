namespace OSK.Transmissions.Messages.Abstractions
{
    public abstract class MessageBase : IMessage
    {
        public string TopicId { get; set; }
    }
}
