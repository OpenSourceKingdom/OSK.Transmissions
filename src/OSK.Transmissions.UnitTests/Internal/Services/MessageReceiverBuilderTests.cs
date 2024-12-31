using System.Security.Cryptography;
using Moq;
using OSK.Transmissions.Internal;
using OSK.Transmissions.Internal.Services;
using OSK.Transmissions.UnitTests.Helpers;
using Xunit;

namespace OSK.Transmissions.UnitTests.Internal.Services
{
    public class MessageReceiverBuilderTests
    {
        #region Variables

        private Mock<IServiceProvider> _mockServiceProvider;
        private MessageReceiverBuilder _builder;

        #endregion

        #region Use

        [Fact]
        public void Use_NullAction_ThrowsArgumentNullException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [1, HashAlgorithmName.SHA384, new TestSettings()]);

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.Use(null));
        }

        [Fact]
        public void Use_ValidAction_ReturnsSuccessfully()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [1, HashAlgorithmName.SHA384, new TestSettings()]);

            // Act/Assert
            _builder.Use(next => next);
        }

        #endregion

        #region BuildReceiver

        [Fact]
        public void BuildReceiver_NoTransmissionDelegationHandling_ThrowsInvalidOperationException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [1, HashAlgorithmName.SHA384, new TestSettings()]);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(_builder.BuildReceiver);
        }

        [Fact]
        public async Task BuildReceiver_TransmissionDelegationDoesNotHandleTransmission_DelegateUsageThrowsInvalidOperationException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [1, HashAlgorithmName.SHA384, new TestSettings()]);

            _builder.Use(next => next);

            // Act
            var receiver = (TestMessageReceiver)_builder.BuildReceiver();

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                receiver.TransmissionDelegate(new MessageTransmissionContext<TestMessage>(_mockServiceProvider.Object, new TestMessage(), null)));
        }

        [Fact]
        public async Task BuildReceiver_Valid_ReturnsTestReceiverWithExpectedParameters()
        {
            // Arrange
            var testInt = 117;
            var hashAlgorithmName = HashAlgorithmName.SHA1;
            var receiverId = "abc";
            var settings = new TestSettings();

            SetupVariables<TestMessageReceiver>(receiverId, [testInt, hashAlgorithmName, settings]);

            _builder.Use(next => context =>
            {
                return Task.CompletedTask;
            });

            // Act
            var receiver = _builder.BuildReceiver();

            // Assert
            Assert.IsType<TestMessageReceiver>(receiver);
            var testReceiver = (TestMessageReceiver)receiver;

            Assert.Equal(receiverId, testReceiver.ReceiverId);
            Assert.Equal(testInt, testReceiver.A);
            Assert.Equal(hashAlgorithmName, testReceiver.AlgorithmName);
            Assert.Equal(settings, testReceiver.Settings);

            await testReceiver.TransmissionDelegate(new MessageTransmissionContext<TestMessage>(_mockServiceProvider.Object, new TestMessage(), null));
        }

        [Fact]
        public async Task BuildReceiver_Valid_ReturnsTestReceiverWithExpectedDelegateInCorrectMiddlewareOrder()
        {
            // Arrange
            var testInt = 117;
            var hashAlgorithmName = HashAlgorithmName.SHA1;
            var receiverId = "abc";
            var settings = new TestSettings();

            SetupVariables<TestMessageReceiver>(receiverId, [testInt, hashAlgorithmName, settings]);

            var counter = 0;
            _builder.UseMiddleware((context, next) =>
            {
                counter += 1;
                return next(context);
            });
            _builder.UseMiddleware((context, next) =>
            {
                counter *= 2;
                return next(context);
            });
            _builder.UseMiddleware((context, next) =>
            {
                counter -= 1;
                return next(context);
            });
            _builder.Use(next => _ =>
            {
                return Task.CompletedTask;
            });

            // Act
            var receiver = _builder.BuildReceiver();

            // Assert
            Assert.IsType<TestMessageReceiver>(receiver);
            var testReceiver = (TestMessageReceiver)receiver;

            Assert.Equal(receiverId, testReceiver.ReceiverId);
            Assert.Equal(testInt, testReceiver.A);
            Assert.Equal(hashAlgorithmName, testReceiver.AlgorithmName);
            Assert.Equal(settings, testReceiver.Settings);

            await testReceiver.TransmissionDelegate(new MessageTransmissionContext<TestMessage>(_mockServiceProvider.Object, new TestMessage(), null));

            // Middlewares should go first 
            Assert.Equal(1, counter);
        }

        #endregion

        #region Helpers

        private void SetupVariables<T>(string receiverId, params object[] parameters)
        {
            _mockServiceProvider = new Mock<IServiceProvider>();

            var descriptor = new MessageReceiverDescriptor(receiverId, typeof(T), parameters);
            _builder = new MessageReceiverBuilder(_mockServiceProvider.Object, descriptor);
        }

        #endregion
    }
}
