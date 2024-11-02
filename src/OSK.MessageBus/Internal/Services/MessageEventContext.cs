using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Events.Abstractions;
using System;

namespace OSK.MessageBus.Internal.Services
{
    public class MessageEventContext<TMessage>(IServiceProvider services, TMessage message, object? rawMessage): IMessageEventContext<TMessage>
        where TMessage: IMessageEvent
    {
        #region Variables

        public object? RawMessage => rawMessage;

        public IServiceProvider Services => services;

        public IMessageEvent Message => message;

        TMessage IMessageEventContext<TMessage>.Message => message;

        #endregion
    }
}
