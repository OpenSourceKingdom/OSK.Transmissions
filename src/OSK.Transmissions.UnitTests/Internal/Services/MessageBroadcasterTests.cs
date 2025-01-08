using Moq;
using OSK.Functions.Outputs.Abstractions;
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
        public async Task BroadcastMessageAsync_AllTransmittersFail_ReturnsMultipleOutputs()
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
            Assert.True(result.IsSuccessful);
            Assert.Equal(OutputSpecificityCode.MultipleOutputs, result.StatusCode.SpecificityCode);

            _ = result.Outputs.Single(output => output.ErrorInformation?.Exception is not null &&
                output.ErrorInformation.Value.Exception is NotSupportedException);
            _ = result.Outputs.Single(output => output.ErrorInformation?.Exception is not null &&
                output.ErrorInformation.Value.Exception is InvalidOperationException);
        }

        [Fact]
        public async Task BroadcastMessageAsync_SomeTransmittersFailSomeSucceed_ReturnsMultipleOutputs()
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
            Assert.Equal(OutputSpecificityCode.MultipleOutputs, result.StatusCode.SpecificityCode);
            Assert.Equal(1, result.Outputs.Count(output => output.IsSuccessful));
            Assert.Equal(1, result.Outputs.Count(output => !output.IsSuccessful));

            var transmissionResultA = result.Outputs.First(output => output.IsSuccessful && output.Value.TransmitterId == "TransmitterA");
            Assert.True(transmissionResultA.IsSuccessful);

            var transmissionResultB = result.Outputs.First(output => output.Value.TransmitterId == "TransmitterB");
            Assert.False(transmissionResultB.IsSuccessful);
            Assert.IsType<InvalidOperationException>(transmissionResultB.ErrorInformation.Value.Exception);
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
            Assert.Equal(OutputSpecificityCode.MultipleOutputs, result.StatusCode.SpecificityCode);
            Assert.Equal(2, result.Outputs.Count(output => output.IsSuccessful));

            var transmissionResultA = result.Outputs.First(output => output.IsSuccessful && output.Value.TransmitterId == "TransmitterA");
            Assert.True(transmissionResultA.IsSuccessful);

            var transmissionResultB = result.Outputs.First(output => output.IsSuccessful && output.Value.TransmitterId == "TransmitterB");
            Assert.True(transmissionResultB.IsSuccessful);
        }

        #endregion
    }
}
