using System.Collections.Generic;

namespace OSK.MessageBus.Abstractions
{
    public class BroadcastResult
    {
        public IEnumerable<TransmissionResult> TransmissionResults { get; set; }
    }
}
