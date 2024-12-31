using OSK.Transmissions.Abstractions;
using System.Threading.Tasks;

namespace OSK.Transmissions.Models
{
    public delegate Task MessageTransmissionDelegate(IMessageTransmissionContext transmissionContext);
}
