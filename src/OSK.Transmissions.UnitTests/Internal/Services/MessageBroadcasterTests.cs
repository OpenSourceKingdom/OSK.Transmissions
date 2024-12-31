using Moq;
using OSK.Functions.Outputs.Logging.Abstractions;
using OSK.Functions.Outputs.Mocks;
using OSK.Transmissions.Abstractions;
using OSK.Transmissions.Internal;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.UnitTests.Helpers;
using System.Net;
using Xunit;

namespace OSK.Transmissions.UnitTests.Internal.Services
{
    public class MessageBroadcasterTests
    {
        #region Variables

        private readonly List<MessageTransmitterDescriptor> _descriptors;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly IOutputFactory<MessageBroadcaster> _outputFactory;

        private readonly MessageBroadcaster _broadcaster;

        #endregion

        #region Constructors

        public MessageBroadcasterTests()
        {
            _descriptors = [];
            _mockServiceProvider = new Mock<IServiceProvider>();
            _outputFactory = new MockOutputFactory<MessageBroadcaster>();

            _broadcaster = new MessageBroadcaster(_descriptors, _mockServiceProvider.Object, _outputFactory);
        }

        #endregion

        #region BroadcastMessageAsync

        [Fact]
        public async Task BroadcastMessageAsync_NullOptions_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _broadcaster.BroadcastMessageAsync(new TestMessage(), null));
        }

        [Fact]
        public async Task BroadcastMessageAsync_AllTransmittersFail_ReturnsInternalServerErrorWithAggregateException()
        {
            // Arrange
            var descriptorA = new MessageTransmitterDescriptor("TransmitterA", typeof(TestTransmitterA));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterA)))
                .Returns(new TestTransmitterA()
                {
                    ExceptionToThrow = new NotSupportedException()
                });

            var descriptorB = new MessageTransmitterDescriptor("TransmitterB", typeof(TestTransmitterB));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterB)))
                .Returns(new TestTransmitterB()
                {
                    ExceptionToThrow = new InvalidOperationException()
                });

            _descriptors.Add(descriptorA);
            _descriptors.Add(descriptorB);

            // Act
            var result = await _broadcaster.BroadcastMessageAsync(new TestMessage());

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.Code.StatusCode);

            Assert.IsType<AggregateException>(result.ErrorInformation.Value.Exception);
            var aggregateException = (AggregateException)result.ErrorInformation.Value.Exception;

            _ = aggregateException.InnerExceptions.Single(static exception => exception is NotSupportedException);
            _ = aggregateException.InnerExceptions.Single(static exception => exception is InvalidOperationException);
        }

        [Fact]
        public async Task BroadcastMessageAsync_SomeTransmittersFailSomeSucceed_ReturnsMultiStatus()
        {
            // Arrange
            var descriptorA = new MessageTransmitterDescriptor("TransmitterA", typeof(TestTransmitterA));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterA)))
                .Returns(new TestTransmitterA());

            var descriptorB = new MessageTransmitterDescriptor("TransmitterB", typeof(TestTransmitterB));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterB)))
                .Returns(new TestTransmitterB()
                {
                    ExceptionToThrow = new InvalidOperationException()
                });

            _descriptors.Add(descriptorA);
            _descriptors.Add(descriptorB);

            // Act
            var result = await _broadcaster.BroadcastMessageAsync(new TestMessage());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.MultiStatus, result.Code.StatusCode);
            Assert.Equal(1, result.Value.TransmissionResults.Count(transmission => transmission.Successful));
            Assert.Equal(1, result.Value.TransmissionResults.Count(transmission => !transmission.Successful));

            var transmissionResultA = result.Value.TransmissionResults.First(transmission => transmission.TransmitterId == "TransmitterA");
            Assert.True(transmissionResultA.Successful);

            var transmissionResultB = result.Value.TransmissionResults.First(transmission => transmission.TransmitterId == "TransmitterB");
            Assert.False(transmissionResultB.Successful);
            Assert.IsType<InvalidOperationException>(transmissionResultB.Exception);
        }

        [Fact]
        public async Task BroadcastMessageAsync_AllTransmmitersSucceed_ReturnsSuccess()
        {
            // Arrange
            var descriptorA = new MessageTransmitterDescriptor("TransmitterA", typeof(TestTransmitterA));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterA)))
                .Returns(new TestTransmitterA());

            var descriptorB = new MessageTransmitterDescriptor("TransmitterB", typeof(TestTransmitterB));
            _mockServiceProvider.Setup(m => m.GetService(typeof(TestTransmitterB)))
                .Returns(new TestTransmitterB());

            _descriptors.Add(descriptorA);
            _descriptors.Add(descriptorB);

            // Act
            var result = await _broadcaster.BroadcastMessageAsync(new TestMessage());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.Code.StatusCode);
            Assert.Equal(2, result.Value.TransmissionResults.Count(transmission => transmission.Successful));

            var transmissionResultA = result.Value.TransmissionResults.First(transmission => transmission.TransmitterId == "TransmitterA");
            Assert.True(transmissionResultA.Successful);

            var transmissionResultB = result.Value.TransmissionResults.First(transmission => transmission.TransmitterId == "TransmitterB");
            Assert.True(transmissionResultB.Successful);
        }

        #endregion
    }
}
