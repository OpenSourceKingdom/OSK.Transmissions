using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Messages.Abstractions;

namespace OSK.Transmissions.UnitTests.Helpers
{
    public class TestTransmitterA : IMessageTransmitter
    {
        public Exception ExceptionToThrow { get; set; }

        public Task TransmitAsync<TMessage>(TMessage message, MessageTransmissionOptions options, CancellationToken cancellationToken = default)
            where TMessage : IMessage
        {
            return ExceptionToThrow is null
                ? Task.CompletedTask
                : throw ExceptionToThrow;
        }
    }
}
