using System;
using System.Collections.Generic;
using System.Text;

namespace WebSphereMqClientTest
{
    public class QueueOptions
    {
        public QueueAccess AccessLevel { get; internal set; }
        public string Channel { get; internal set; }
        public string HostName { get; internal set; }
        public int Port { get; internal set; }
        public string QueueManagerName { get; internal set; }
        public string QueueName { get; internal set; }
        public int WaitMilliseconds { get; internal set; }
    }
}
