using Microsoft.Extensions.Options;
using Moq;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.Options;
using OSK.Transmissions.UnitTests.Helpers;
using Xunit;

namespace OSK.Transmissions.UnitTests.Internal.Services
{
    public class MessageReceiverGroupBuilderTests
    {
        #region Variables

        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly MessageReceiverGroupBuilder<TestMessageReceiver> _builder;

        #endregion

        #region Constructors

        public MessageReceiverGroupBuilderTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();

            var mockOptions = new Mock<IOptions<MessageBusConfigurationOptions>>();
            mockOptions.SetupGet(m => m.Value)
                .Returns(new MessageBusConfigurationOptions());

            _builder = new MessageReceiverGroupBuilder<TestMessageReceiver>(_mockServiceProvider.Object, mockOptions.Object);
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

        #region AddMessageReceiver

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void AddMessageReceiver_InvalidReceiverIds_ThrowsArgumentNullException(string receiverId)
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageReceiver(receiverId, [], _ => { }));
        }

        [Fact]
        public void AddMessageReceiver_NullParameters_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageReceiver("abc", null, _ => { }));
        }

        [Fact]
        public void AddMessageReceiver_NullAction_ThrowsArgumentNullException()
        {
            // Arrange/Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddMessageReceiver("abc", [], null));
        }

        [Fact]
        public void AddMessageReceiver_DuplicateReceiverId_ThrowsInvalidOperationException()
        {
            // Arrange
            _builder.AddMessageReceiver("abc", [], _ => { });

            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => _builder.AddMessageReceiver("abc", [], _ => { }));
        }

        [Fact]
        public void AddMessageReceiver_Valid_ReturnsSuccessfully()
        {
            // Arrange/Act/Assert
            _builder.AddMessageReceiver("abc", [], _ => { });
        }

        #endregion
    }
}
