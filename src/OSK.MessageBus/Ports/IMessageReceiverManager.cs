using OSK.Hexagonal.MetaData;
using System;

namespace OSK.MessageBus.Ports
{
    /// <summary>
    /// Manages the start and stop status' of event receivers that have been added to the dependdency container
    /// </summary>
    [HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
    public interface IMessageReceiverManager: IDisposable
    {
        void Start();

        void Stop();
    }
}
