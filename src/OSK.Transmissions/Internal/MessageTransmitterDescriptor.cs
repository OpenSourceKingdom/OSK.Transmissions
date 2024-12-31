using System;

namespace OSK.Transmissions.Internal
{
    internal class MessageTransmitterDescriptor(string transmitterName, Type transmitterType)
    {
        public string TransmitterId => transmitterName;

        public Type TransmitterType => transmitterType;
    }
}
