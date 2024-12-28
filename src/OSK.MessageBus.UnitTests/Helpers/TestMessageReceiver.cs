using System.Security.Cryptography;
using OSK.MessageBus.Models;
using OSK.MessageBus.Ports;

namespace OSK.MessageBus.UnitTests.Helpers
{
    public class TestMessageReceiver(string receiverId, MessageEventTransmissionDelegate eventDelegate,
        int a, HashAlgorithmName algorithmName, TestSettings settings) : IMessageEventReceiver
    {
        public string ReceiverId = receiverId;
        public int A => a;
        public HashAlgorithmName AlgorithmName => algorithmName;
        public TestSettings Settings => settings;
        public MessageEventTransmissionDelegate EventDelegate => eventDelegate;

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
