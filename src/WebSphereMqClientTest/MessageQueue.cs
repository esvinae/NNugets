using IBM.WMQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WebSphereMqClientTest
{
    public class MessageQueue : IDisposable
    {
        private readonly QueueOptions _queueOptions;

        private MQQueueManager _queueManager;
        private MQQueue _queue;

        public MessageQueue(QueueOptions queueOptions)
        {
            _queueOptions = queueOptions;
        }

        public bool IsAttached { get; private set; }

        public string QueueName
        {
            get { return _queueOptions.QueueName; }
        }

        public void Attach()
        {
            var properties = new Hashtable
            {
                {MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED},
                {MQC.HOST_NAME_PROPERTY, _queueOptions.HostName},
                {MQC.PORT_PROPERTY, _queueOptions.Port},
                {MQC.CHANNEL_PROPERTY, _queueOptions.Channel},
            };

            try
            {
                Console.WriteLine("Queue {0} is connecting to queue manager {1}", QueueName, _queueOptions.QueueManagerName);
                _queueManager = new MQQueueManager(_queueOptions.QueueManagerName, properties);

                int openOptions = MQC.MQOO_FAIL_IF_QUIESCING;
                if ((_queueOptions.AccessLevel & QueueAccess.Get) == QueueAccess.Get)
                {
                    openOptions += MQC.MQOO_INPUT_AS_Q_DEF;
                }
                if ((_queueOptions.AccessLevel & QueueAccess.Put) == QueueAccess.Put)
                {
                    openOptions += MQC.MQOO_OUTPUT;
                }
                if ((_queueOptions.AccessLevel & QueueAccess.Browse) == QueueAccess.Browse)
                {
                    openOptions += MQC.MQOO_INQUIRE;
                }
                Console.WriteLine("Accessing queue {0} with options {1:G}...", QueueName, _queueOptions.AccessLevel);

                _queue = _queueManager.AccessQueue(_queueOptions.QueueName, openOptions);

                Console.WriteLine("Queue {0} is successfully attached.", QueueName);

                // reset point for transactional support
                IsAttached = true;
            }

            catch (MQException)
            {
                // wrap exception
                throw;
            }
        }

        public void Dispose()
        {
            if (IsAttached)
            {
                Detach();
            }
        }

        public void Detach()
        {
            Console.WriteLine("Detaching from queue {0}...", QueueName);

            // transactions: add wait 

            // transactions: rollback wait check

            ActIfNotNull(_queue, () => _queue.Close(), "close");
            ActIfNotNull(_queueManager, () => _queueManager.Disconnect(), "disconnect");

            _queue = null;
            _queueManager = null;

            IsAttached = false;
        }
        
        private void ActIfNotNull<T>(T entity, Action action, string operation) where T : class
        {
            try
            {
                if (entity != null)
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error performing operation '{0}' for queue {1}...", operation, QueueName, ex);
            }
        }
        
        public void PutMessageBlock(string messageContent)
        {
            var putMessageOptions = new MQPutMessageOptions();
            // add for transactions
            // putMessageOptions.Options += MQC.MQPMO_SYNCPOINT;
            PutMessage(messageContent, putMessageOptions);

            // commit transaction
        }

        private void PutMessage(string messageContent, MQPutMessageOptions putMessageOptions)
        {
            try
            {
                var message = new MQMessage();
                message.Write(Encoding.UTF8.GetBytes(messageContent));
                _queue.Put(message, putMessageOptions);
            }

            catch (MQException)
            {
                // wrap exception
                throw;
            }
        }

        public string GetMessageBlock()
        {
            var getMessageOptions = new MQGetMessageOptions();
            // transactions: 
            // getMessageOptions.Options += MQC.MQGMO_SYNCPOINT;
            getMessageOptions.Options += MQC.MQGMO_WAIT;
            getMessageOptions.WaitInterval = _queueOptions.WaitMilliseconds;
            return GetMessage(getMessageOptions);
        }

        private string GetMessage(MQGetMessageOptions getMessageOptions)
        {
            // transactions: add wait

            // transactions: add rollback wait check

            try
            {
                // if transaction exists, add transaction complete handler
                var message = new MQMessage();
                _queue.Get(message, getMessageOptions);
                var result = message.ReadString(message.MessageLength);
                message.ClearMessage();

                return result;
            }

            catch (MQException mqe)
            {
                // queue empty
                if (mqe.ReasonCode == 2033)
                {
                    return null;
                }
                // wrap exception
                throw;
            }
        }
    }
}
