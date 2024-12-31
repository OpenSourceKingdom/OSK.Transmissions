using System.Collections.Generic;

namespace OSK.Transmissions.Abstractions
{
    public class BroadcastResult
    {
        public IEnumerable<MessageTransmissionResult> TransmissionResults { get; set; }
    }
}
