using Moq;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.Ports;
using Xunit;

namespace OSK.Transmissions.UnitTests.Internal.Services
{
    public class MessageReceiverManagerTests
    {
        #region Variables

        private readonly Mock<IMessageReceiverGroupBuilder> _transmissionBuilder;
        private readonly MessageReceiverManager _manager;

        #endregion

        #region Constructors

        public MessageReceiverManagerTests()
        {
            _transmissionBuilder = new Mock<IMessageReceiverGroupBuilder>();

            _manager = new MessageReceiverManager([_transmissionBuilder.Object]);
        }

        #endregion

        #region Start

        [Fact]
        public void Start_ConfiguresManager_RunsTwice_ConfiguresOnlyOnce()
        {
            // Arrange
            var mockReceiver = new Mock<IMessageReceiver>();

            _transmissionBuilder.Setup(m => m.BuildReceivers())
                .Returns([mockReceiver.Object]);

            // Act
            _manager.Start();
            _manager.Start();

            // Assert
            _transmissionBuilder.Verify(m => m.BuildReceivers(), Times.Once);
            mockReceiver.Verify(m => m.Start(), Times.Once);
        }

        #endregion

        #region Stop

        [Fact]
        public void Stop_MultipleCalls_RunsOnce_MultipleReceiversStop_SomeThrowExceptions_StopsAllReceiversAndThrowsAggregateException()
        {
            // Arrange
            var receiverA = new Mock<IMessageReceiver>();
            var receiverB = new Mock<IMessageReceiver>();
            var receiverC = new Mock<IMessageReceiver>();

            receiverB.Setup(m => m.Dispose())
                .Throws<InvalidOperationException>();

            _transmissionBuilder.Setup(m => m.BuildReceivers())
                .Returns([receiverA.Object, receiverB.Object, receiverC.Object]);

            _manager.Start();

            // Act/Assert
            Assert.Throws<AggregateException>(_manager.Stop);
            _manager.Stop();

            receiverA.Verify(m => m.Dispose(), Times.Once);
            receiverB.Verify(m => m.Dispose(), Times.Once);
            receiverC.Verify(m => m.Dispose(), Times.Once);
        }

        [Fact]
        public void Stop_MultipleCalls_RunsOnce_MultipleReceiversStop_NoExceptions_StopsAllReceiversSuccessfully()
        {
            // Arrange
            var receiverA = new Mock<IMessageReceiver>();
            var receiverB = new Mock<IMessageReceiver>();
            var receiverC = new Mock<IMessageReceiver>();

            _transmissionBuilder.Setup(m => m.BuildReceivers())
                .Returns([receiverA.Object, receiverB.Object, receiverC.Object]);

            _manager.Start();

            // Act
            _manager.Stop();
            _manager.Stop();

            // Assert
            receiverA.Verify(m => m.Dispose(), Times.Once);
            receiverB.Verify(m => m.Dispose(), Times.Once);
            receiverC.Verify(m => m.Dispose(), Times.Once);
        }

        #endregion
    }
}
