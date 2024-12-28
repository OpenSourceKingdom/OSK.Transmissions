namespace OSK.MessageBus.Events.Abstractions

    /// <summary>
    /// Represents a generic message that can be sent through a message bus.
    /// </summary>
    public interface IMessageEvent
    {
        /// <summary>
        /// The specific topic or category to which the message is associated to
        /// </summary>
        public string TopicId { get; }
    }
}
