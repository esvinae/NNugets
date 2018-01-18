using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace WebSphereMqClientTest
{
    [TestClass]
    public class MessageQueueTest
    {
        [TestMethod]
        public void TestGetSet()
        {
            QueueOptions options = new QueueOptions();
            options.AccessLevel = QueueAccess.Put | QueueAccess.Get;
            int portNumber = 1414;
            int.TryParse(ConfigurationManager.AppSettings["port"], out portNumber);
            options.Port = portNumber;
            options.HostName = ConfigurationManager.AppSettings["host"];
            options.QueueManagerName = ConfigurationManager.AppSettings["manager"];
            options.Channel = ConfigurationManager.AppSettings["channel"];
            options.QueueName = ConfigurationManager.AppSettings["queue"];
            options.WaitMilliseconds = 1000;

            var message = Guid.NewGuid().ToString();
            string response = null;
            using (MessageQueue client = new MessageQueue(options))
            {
                client.Attach();
                client.PutMessageBlock(message);
                response = client.GetMessageBlock();
            }
            Assert.AreEqual(message, response);
        }
    }
}
