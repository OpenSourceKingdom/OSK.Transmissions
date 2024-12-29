using OSK.MessageBus.Abstractions;
using System.Threading.Tasks;

namespace OSK.MessageBus.Models
{
    public delegate Task MessageTransmissionDelegate(IMessageTransmissionContext transmissionContext);
}
