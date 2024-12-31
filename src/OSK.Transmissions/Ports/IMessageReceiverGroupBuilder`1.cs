using System;
using OSK.Hexagonal.MetaData;
using OSK.Transmissions;

namespace OSK.Transmissions.Ports
{
    /// <summary>
    /// Helps to construct a set of receivers that are tied to given transmitter for message transmission.
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageReceiverGroupBuilder<TReceiver> : IMessageReceiverGroupBuilder
        where TReceiver : IMessageReceiver
    {
        /// <summary>
        /// Adds a message receiver of type <see cref="{TReceiver}"/> to the message transmission channel with the given id and custom parameters
        /// </summary>
        /// <param name="receiverId">A unique id</param>
        /// <param name="parameters">
        /// An array of arguments to provide to message receivers when they are constructed.
        /// 
        /// Note: there are expected arguments that a message receiver should take in order to fully integrate with the library. 
        /// See <see cref="MessageReceiverBase"/> for these parameters
        /// </param>
        /// <param name="receiverBuilder">The builder for custom receiver middleware configuration</param>
        /// <returns>The group builder for chaining</returns>
        IMessageReceiverGroupBuilder<TReceiver> AddMessageReceiver(string receiverId, object[] parameters,
            Action<IMessageReceiverBuilder> receiverBuilder);

        /// <summary>
        /// Adds a message receiver to the message transmission channel with the given id and custom parameters
        /// </summary>
        /// <param name="receiverId">A unique id</param>
        /// <param name="parameters">
        /// An array of arguments to provide to message receivers when they are constructed.
        /// 
        /// Note: there are expected arguments that a message receiver should take in order to fully integrate with the library. 
        /// See <see cref="MessageReceiverBase"/> for these parameters
        /// </param>
        /// <param name="receiverBuilder">The builder for custom receiver middleware configuration</param>
        /// <returns>The group builder for chaining</returns>
        IMessageReceiverGroupBuilder<TReceiver> AddMessageReceiver<TChildReceiver>(string receiverId, object[] parameters,
            Action<IMessageReceiverBuilder> receiverBuilder)
            where TChildReceiver : TReceiver;
    }
}
