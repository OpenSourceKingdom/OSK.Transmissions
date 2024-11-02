using Moq;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Ports;
using Xunit;

namespace OSK.MessageBus.RabbitMQ.UnitTests.Internal.Services
{
    public class MessageEventReceiverManagerTests
    {
        #region Variables

        private readonly IMessageEventReceiverManager _manager;

        #endregion

        #region Constructors

        public MessageEventReceiverManagerTests()
        {
            _manager = new MessageEventReceiverManager(Mock.Of<IServiceProvider>());
        }

        #endregion

        #region AddConfigurator

        [Fact]
        public void AddConfigurator_NullAction_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _manager.AddConfigurator(null));
        }

        [Fact]
        public void AddConfigurator_ValidAction_ReturnsSuccessfully()
        {
            // Arrange/Act/Assert
            _manager.AddConfigurator(_ => { });
        }

        #endregion

        #region AddEventReceiver

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void AddEventReceiver_InvalidSubscriptionId_ThrowsArgumentException(string subscriptionId)
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentException>(() => _manager.AddEventReceiver(subscriptionId,
                (_, _) => Mock.Of<IMessageEventReceiverBuilder>()));
        }

        [Fact]
        public void AddEventReceiver_DuplicateSubscriptionId_ThrowsInvalidOperationException()
        {
            // Arrange/Act
            _manager.AddEventReceiver("Abc", (_, _) => Mock.Of<IMessageEventReceiverBuilder>());

            // Assert
            Assert.Throws<InvalidOperationException>(() => 
            _manager.AddEventReceiver("Abc", (_, _) => Mock.Of<IMessageEventReceiverBuilder>()));
        }

        #endregion

        #region Start

        [Fact]
        public void Start_ConfiguresManager_RunsTwice_ConfiguresOnlyOnce()
        {
            // Arrange
            var mockReceiver = new Mock<IMessageEventReceiver>();
            var builder = new Mock<IMessageEventReceiverBuilder>();
            builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                .Returns(mockReceiver.Object);

            _manager.AddEventReceiver("Abc", (_, _) => builder.Object);

            // Act
            _manager.Start();
            _manager.Start();

            // Asseer
            builder.Verify(m => m.BuildReceiver(It.IsAny<string>()), Times.Once);
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

            _manager.AddEventReceiver("A", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverA.Object);

                return builder.Object;
            });
            _manager.AddEventReceiver("B", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverB.Object);

                return builder.Object;
            });
            _manager.AddEventReceiver("C", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverC.Object);

                return builder.Object;
            });

            _manager.Start();

            // Act/Assert
            Assert.Throws<AggregateException>(_manager.Stop);
            _manager.Stop();

            receiverA.Verify(m =>  m.Dispose(), Times.Once);
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

            _manager.AddEventReceiver("A", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverA.Object);

                return builder.Object;
            });
            _manager.AddEventReceiver("B", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverB.Object);

                return builder.Object;
            });
            _manager.AddEventReceiver("C", (_, _) =>
            {
                var builder = new Mock<IMessageEventReceiverBuilder>();
                builder.Setup(m => m.BuildReceiver(It.IsAny<string>()))
                    .Returns(receiverC.Object);

                return builder.Object;
            });

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
