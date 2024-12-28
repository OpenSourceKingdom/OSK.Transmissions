using Microsoft.Extensions.Options;
using Moq;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Options;
using OSK.MessageBus.UnitTests.Helpers;
using Xunit;

namespace OSK.MessageBus.UnitTests.Internal.Services
{
    public class MessageTransmissionBuilderTests
    {
        #region Variables

        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly MessageTransmissionBuilder<TestMessageReceiver> _builder;

        #endregion

        #region Constructors

        public MessageTransmissionBuilderTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();

            var mockOptions = new Mock<IOptions<MessageBusConfigurationOptions>>();
            mockOptions.SetupGet(m => m.Value)
                .Returns(new MessageBusConfigurationOptions());

            _builder = new MessageTransmissionBuilder<TestMessageReceiver>(_mockServiceProvider.Object, mockOptions.Object);
        }

        #endregion

        #region AddConfigurator

        [Fact]
        public void AddConfigurator_NullAction_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddConfigurator(null));
        }

        [Fact]
        public void AddConfigurator_ValidAction_ReturnsSuccessfully()
        {
            // Arrange/Act/Assert
            _builder.AddConfigurator(_ => { });
        }

        #endregion

        #region AddMessageEventReceiver

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void AddMessageEventReceiver_InvalidReceiverIds_ThrowsArgumentNullException(string receiverId)
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageEventReceiver(receiverId, [], _ => { }));
        }

        [Fact]
        public void AddMessageEventReceiver_NullParameters_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageEventReceiver("abc", null, _ => { }));
        }

        [Fact]
        public void AddMessageEventReceiver_NullAction_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageEventReceiver("abc", [], null));
        }

        [Fact]
        public void AddMessageEventReceiver_DuplicateReceiverId_ThrowsInvalidOperationException()
        {
            // Arrange
            _builder.AddMessageEventReceiver("abc", [], _ => { });

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() =>  _builder.AddMessageEventReceiver("abc", [], _ => { }));
        }

        [Fact]
        public void AddMessageEventReceiver_Valid_ReturnsSuccessfully()
        {
            // Arrange/Act/Assert
            _builder.AddMessageEventReceiver("abc", [], _ => { });
        }

        #endregion
    }
}
