using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus.UnitTests.Helpers
{
    public class TestEventSink : MessageEventSinkBase
    {
        public const string TestEventSinkSource = "Test";

        public TestEventSink(IMessageEventPublisher publisher, IOutputFactory<MessageEventSinkBase> outputFactory) 
            : base(publisher, outputFactory, TestEventSinkSource)
        {
        }
    }
}
