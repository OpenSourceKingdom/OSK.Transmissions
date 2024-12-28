using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;

namespace OSK.MessageBus.Internal.Services
{
    public class MessageTransmissionContext<TMessage>(IServiceProvider services, TMessage message, object? rawMessage): IMessageTransmissionContext<TMessage>
        where TMessage: IMessage
    {
        #region Variables

        public object? RawMessage => rawMessage;

        public IServiceProvider Services => services;

        public IMessage Message => message;

        TMessage IMessageTransmissionContext<TMessage>.Message => message;

        #endregion
    }
}
