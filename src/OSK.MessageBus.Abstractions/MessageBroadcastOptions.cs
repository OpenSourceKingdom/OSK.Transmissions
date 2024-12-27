using System.Collections.Generic;

namespace OSK.MessageBus.Abstractions
{
    public class MessageBroadcastOptions
    {
        public MessageTransmissionOptions TransmissionOptions { get; set; }

        public ISet<string> TransmitterTargetIds { get; set; }
    }
}
