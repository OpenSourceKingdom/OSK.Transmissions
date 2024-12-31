using OSK.Transmissions.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Transmissions.Internal.Services
{
    internal class MessageReceiverManager(IEnumerable<IMessageReceiverGroupBuilder> receiverGroupBuilders)
        : IMessageReceiverManager
    {
        #region Variables

        private List<IMessageReceiver> _receivers = [];
        private bool _started;

        #endregion

        #region IMessageReceiverManager

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
                catch (Exception ex)
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
            => receiverGroupBuilders.SelectMany(builder => builder.BuildReceivers());

        #endregion
    }
}
