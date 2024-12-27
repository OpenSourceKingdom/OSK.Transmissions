using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;

namespace OSK.MessageBus.UnitTests.Helpers
{
    public class TestTransmitterB : IMessageEventTransmitter
    {
        public Exception ExceptionToThrow { get; set; }

        public Task TransmitAsync<TMessage>(TMessage message, MessageTransmissionOptions options, CancellationToken cancellationToken = default)
            where TMessage : IMessageEvent
        {
            return ExceptionToThrow is null
                ? Task.CompletedTask
                : throw ExceptionToThrow;
        }
    }
}
