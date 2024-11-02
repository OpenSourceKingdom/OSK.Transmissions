using OSK.MessageBus.Events.Abstractions;

namespace OSK.MessageBus.UnitTests.Helpers
{
    public class TestEvent : IMessageEvent
    {
        public string TopicId => throw new NotImplementedException();
    }
}
