using System.Collections.Generic;

namespace OSK.Transmissions.Abstractions
{
    public class MessageBroadcastOptions
    {
        public MessageTransmissionOptions TransmissionOptions { get; set; }

        public ISet<string> TargetTransmitterIds { get; set; }
    }
}
