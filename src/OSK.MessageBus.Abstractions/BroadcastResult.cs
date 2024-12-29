using System.Collections.Generic;

namespace OSK.MessageBus.Abstractions
{
    public class BroadcastResult
    {
        public IEnumerable<MessageTransmissionResult> TransmissionResults { get; set; }
    }
}
