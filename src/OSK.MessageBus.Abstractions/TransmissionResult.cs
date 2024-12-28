using System;

namespace OSK.MessageBus.Abstractions
{
    public class TransmissionResult
    {
        public string TransmitterId { get; set; } = string.Empty;

        public bool Successful => Exception is null;

        public Exception Exception { get; set; }
    }
}
