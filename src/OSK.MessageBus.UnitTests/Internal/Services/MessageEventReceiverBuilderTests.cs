﻿using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSK.MessageBus.Internal;
using OSK.MessageBus.Internal.Services;
using OSK.MessageBus.Ports;
using OSK.MessageBus.UnitTests.Helpers;
using Xunit;

namespace OSK.MessageBus.UnitTests.Internal.Services
{
    public class MessageEventReceiverBuilderTests
    {
        #region Variables

        private Mock<IServiceProvider> _mockServiceProvider;
        private MessageEventReceiverBuilder _builder;

        #endregion

        #region Use

        [Fact]
        public void Use_NullAction_ThrowsArgumentNullException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [ 1, HashAlgorithmName.SHA384, new TestSettings() ]);

            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => _builder.Use(null));
        }

        [Fact]
        public void Use_ValidAction_ReturnsSuccessfully()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [ 1, HashAlgorithmName.SHA384, new TestSettings() ]);

            // Act/Assert
            _builder.Use(next => next);
        }

        #endregion

        #region BuildReceiver

        [Fact]
        public void BuildReceiver_NoEventDelegationHandling_ThrowsInvalidOperationException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [ 1, HashAlgorithmName.SHA384, new TestSettings() ]);

            // Act/Assert
            Assert.Throws<InvalidOperationException>(_builder.BuildReceiver);
        }

        [Fact]
        public async Task BuildReceiver_EventDelegationDoesNotHandleEvent_DelegateUsageThrowsInvalidOperationException()
        {
            // Arrange
            SetupVariables<TestMessageReceiver>("abc", [ 1, HashAlgorithmName.SHA384, new TestSettings() ]);

            _builder.Use(next => next);

            // Act
            var receiver = (TestMessageReceiver) _builder.BuildReceiver();

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                receiver.EventDelegate(new MessageEventTransmissionContext<TestEvent>(_mockServiceProvider.Object, new TestEvent(), null)));
        }

        [Fact]
        public async Task BuildReceiver_Valid_ReturnsTestReceiverWithExpectedParameters()
        {
            // Arrange
            var testInt = 117;
            var hashAlgorithmName = HashAlgorithmName.SHA1;
            var receiverId = "abc";
            var settings = new TestSettings();

            SetupVariables<TestMessageReceiver>(receiverId, [ testInt, hashAlgorithmName, settings ]);

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

            await testReceiver.EventDelegate(new MessageEventTransmissionContext<TestEvent>(_mockServiceProvider.Object, new TestEvent(), null));
        }

        #endregion

        #region Helpers

        private void SetupVariables<T>(string receiverId, params object[] parameters)
        {
            _mockServiceProvider = new Mock<IServiceProvider>();

            var descriptor = new MessageEventReceiverDescriptor(receiverId, typeof(T), parameters);
            _builder = new MessageEventReceiverBuilder(_mockServiceProvider.Object, descriptor);
        }

        #endregion
    }
}
