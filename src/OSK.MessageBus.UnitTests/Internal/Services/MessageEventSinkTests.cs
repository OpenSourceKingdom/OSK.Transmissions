using Moq;
using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.Functions.Outputs.Mocks;
using OSK.MessageBus.Abstractions;
using OSK.MessageBus.Ports;
using OSK.MessageBus.UnitTests.Helpers;
using System.Net;
using Xunit;

namespace OSK.MessageBus.RabbitMQ.UnitTests.Internal.Services
{
    public class MessageEventSinkTests
    {
        #region Variables

        private readonly Mock<IMessageEventPublisher> _mockBus;
        private readonly IOutputFactory<MessageEventSinkBase> _outputFactory;

        private readonly IMessageEventSink _eventSink;

        #endregion

        #region Constructors

        public MessageEventSinkTests()
        {
            _mockBus = new Mock<IMessageEventPublisher>();
            _outputFactory = new MockOutputFactory<MessageEventSinkBase>();
             
            _eventSink = new TestEventSink(_mockBus.Object, _outputFactory);
        }

        #endregion

        #region PublishAsync

        [Fact]
        public async Task PublishAsync_PassThrough_ReturnsBusOutput()
        {
            // Arrange
            _mockBus.Setup(m => m.PublishAsync(It.IsAny<TestEvent>(), It.IsAny<MessagePublishOptions>(),
                It.IsAny<CancellationToken>()));

            // Act
            var result = await _eventSink.PublishAsync(new TestEvent());

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Code.StatusCode);
        }

        [Fact]
        public async Task PublishAsync_ThrowsException_ReturnsOutputExceptionWithOriginationSource()
        {
            // Arrange
            _mockBus.Setup(m => m.PublishAsync(It.IsAny<TestEvent>(), It.IsAny<MessagePublishOptions>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _eventSink.PublishAsync(new TestEvent());

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code.StatusCode);
            Assert.IsType<InvalidOperationException>(result.ErrorInformation!.Value.Exception);
            Assert.Equal(TestEventSink.TestEventSinkSource, result.Code.OriginationSource);
        }

        #endregion
    }
}
