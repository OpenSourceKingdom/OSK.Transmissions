using OSK.Transmissions.Messages.Abstractions;

namespace OSK.Transmissions.UnitTests.Helpers
{
    public class TestMessage : IMessage
    {
        public string TopicId => throw new NotImplementedException();
    }
}
