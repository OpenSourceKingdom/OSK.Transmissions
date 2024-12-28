using System;
using OSK.Hexagonal.MetaData;

namespace OSK.MessageBus.Ports
{
    /// <summary>
    /// Helps to construct a set of receivers that are tied to given transmitter for message transmission.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageReceiverGroupBuilder<TReceiver>: IMessageReceiverGroupBuilder
        where TReceiver: IMessageReceiver
    {
        /// <summary>
        /// Adds a message event receiver of type <see cref="{TReceiver}"/> to the message transmission channel with the given id and custom parameters
        /// </summary>
        /// <param name="receiverId">A unique id</param>
        /// <param name="parameters">
        /// An array of arguments to provide to message event receivers when they are constructed.
        /// 
        /// Note: there are expected arguments that a message event receiver should take in order to fully integrate with the library. 
        /// See <see cref="MessageEventReceiverBase"/> for these parameters
        /// </param>
        /// <param name="receiverBuilder">The builder for custom receiver middleware configuration</param>
        /// <returns>The group builder for chaining</returns>
        IMessageReceiverGroupBuilder<TReceiver> AddMessageEventReceiver(string receiverId, object[] parameters, 
            Action<IMessageReceiverBuilder> receiverBuilder);

        /// <summary>
        /// Adds a message event receiver to the message transmission channel with the given id and custom parameters
        /// </summary>
        /// <param name="receiverId">A unique id</param>
        /// <param name="parameters">
        /// An array of arguments to provide to message event receivers when they are constructed.
        /// 
        /// Note: there are expected arguments that a message event receiver should take in order to fully integrate with the library. 
        /// See <see cref="MessageEventReceiverBase"/> for these parameters
        /// </param>
        /// <param name="receiverBuilder">The builder for custom receiver middleware configuration</param>
        /// <returns>The group builder for chaining</returns>
        IMessageReceiverGroupBuilder<TReceiver> AddMessageEventReceiver<TChildReceiver>(string receiverId, object[] parameters,
            Action<IMessageReceiverBuilder> receiverBuilder)
            where TChildReceiver: TReceiver;
    }
}
