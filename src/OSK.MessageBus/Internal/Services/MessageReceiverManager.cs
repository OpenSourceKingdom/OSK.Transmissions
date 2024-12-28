using OSK.MessageBus.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.MessageBus.Internal.Services
{
    internal class MessageReceiverManager(IEnumerable<IMessageReceiverGroupBuilder> messageEventTransmissionBuilders) 
        : IMessageReceiverManager
    {
        #region Variables

        private List<IMessageReceiver> _receivers = [];
        private bool _started;

        #endregion

        #region IMessageEventReceiverManager

        public void Start()
        {
            if (_started)
            {
                return;
            }

            _receivers = [];
            foreach (var receiver in BuildReceivers())
            {
                receiver.Start();
                _receivers.Add(receiver);
            }

            _started = true;
        }

        public void Stop()
        {
            if (!_started)
            {
                return;
            }

            var exceptions = new List<Exception>();
            foreach (var receiver in _receivers)
            {
                try
                {
                    receiver.Dispose();
                }
                catch(Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            _receivers.Clear();
            _started = false;

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        public void Dispose()
        {
            Stop();
        }

        #endregion

        #region Helpers

        private IEnumerable<IMessageReceiver> BuildReceivers()
            => messageEventTransmissionBuilders.SelectMany(builder => builder.BuildReceivers());

        #endregion
    }
}
