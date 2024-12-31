using System.Security.Cryptography;
using OSK.Transmissions.Models;
using OSK.Transmissions.Ports;

namespace OSK.Transmissions.UnitTests.Helpers
{
    public class TestMessageReceiver(string receiverId, MessageTransmissionDelegate transmissionDelegate,
        int a, HashAlgorithmName algorithmName, TestSettings settings) : IMessageReceiver
    {
        public string ReceiverId = receiverId;
        public int A => a;
        public HashAlgorithmName AlgorithmName => algorithmName;
        public TestSettings Settings => settings;
        public MessageTransmissionDelegate TransmissionDelegate => transmissionDelegate;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
