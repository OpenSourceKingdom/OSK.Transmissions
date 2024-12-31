using System;

namespace OSK.Transmissions.Abstractions
{
    public class MessageTransmissionResult
    {
        public string TransmitterId { get; set; } = string.Empty;

        public bool Successful => Exception is null;

        public Exception Exception { get; set; }
    }
}
