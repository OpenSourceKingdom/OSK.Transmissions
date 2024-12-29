using OSK.MessageBus.Messages.Abstractions;

namespace OSK.MessageBus.UnitTests.Helpers
{
    public class TestEvent : IMessage
    {
        public string TopicId => throw new NotImplementedException();
    }
}
