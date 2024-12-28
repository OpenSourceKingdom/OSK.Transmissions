﻿using Moq;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Ports;
using Xunit;

namespace OSK.MessageBus.UnitTests.Internal.Services
{
    public class MessageEventReceiverManagerTests
    {
        #region Variables

        private readonly Mock<IMessageEventTransmissionBuilder> _transmissionBuilder;
        private readonly MessageEventReceiverManager _manager;

        #endregion

        #region Constructors

        public MessageEventReceiverManagerTests()
        {
            _transmissionBuilder = new Mock<IMessageEventTransmissionBuilder>();

            _manager = new MessageEventReceiverManager([ _transmissionBuilder.Object ]);
        }

        #endregion

        #region Start

        [Fact]
        public void Start_ConfiguresManager_RunsTwice_ConfiguresOnlyOnce()
        {
            // Arrange
            var mockReceiver = new Mock<IMessageEventReceiver>();

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
            var receiverA = new Mock<IMessageEventReceiver>();
            var receiverB = new Mock<IMessageEventReceiver>();
            var receiverC = new Mock<IMessageEventReceiver>();

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
            var receiverA = new Mock<IMessageEventReceiver>();
            var receiverB = new Mock<IMessageEventReceiver>();
            var receiverC = new Mock<IMessageEventReceiver>();

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
