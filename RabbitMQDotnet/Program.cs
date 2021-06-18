using Microsoft.Practices.Unity;
using RabbitMQ.Client;
using RabbitMQApp.DI;
using RabbitMQApp.RabbitMQHelper;
using System;
using System.Threading;

namespace RabbitMQApp
{
    internal class Program
    {
        private static void Main()
        {
            IConnection connection = null;

            var queueName = Guid.NewGuid().ToString();

            try
            {
                // Printing ASCII Art ;-)
                AsciiArt.AsciiArt.Write("Rabbit MQ");

                // Creating IOC Container
                var container = UnityConfig.GetMainContainer();
                var obj = container.Resolve<IRabbitMQHelper>();

                // Creating Connection and Connection Factory
                var connectionFactory = obj.GetConnectionFactory();
                connection = obj.CreateConnection(connectionFactory);

                // Creating a New Queue
                Console.WriteLine("Creating New Queue");
                obj.CreateQueue(queueName, connection);
                Console.WriteLine($"Queue Sucessfully Created: {queueName}");
                Console.WriteLine(" ");

                // Retrieving Message Count from Queue
                Console.Write("Messages Count: ");
                var messageCount = obj.RetrieveMessageCount(queueName, connection);
                Console.WriteLine(messageCount.ToString());
                Console.WriteLine(" ");

                // Writing Messages to a Queue
                for (int i = 0; i < 100; i++)
                {
                    var newmessage = $"New Message Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss:fff}";

                    obj.WriteMessageOnQueue(newmessage, queueName, connection);
                    Console.WriteLine($"Message Successfully Written: {newmessage}");
                }
                Console.WriteLine(" ");



                // Retrieving Message Count from Queue
                Console.Write("Messages Count: ");
                var messageCountcx = obj.RetrieveMessageCount(queueName, connection);
                Console.WriteLine(messageCountcx.ToString());
                Console.WriteLine(" ");

                // Retrieving One Message
                Console.Write("Retrieving One Message, Message Text: ");
                var message = obj.RetrieveSingleMessage(queueName, connection);
                Console.WriteLine(message);
                Console.WriteLine(" ");

                // Retrieving Multiple Messages
                var lstMessages = obj.RetrieveMessageList(queueName, connection);
                foreach (var m in lstMessages)
                {
                    Console.WriteLine($"Message Text: {m} ");
                }


                // Retrieving Message Count from Queue
                Console.Write("Messages Count: ");
                var messageCountc = obj.RetrieveMessageCount(queueName, connection);
                Console.WriteLine(messageCountc.ToString());
                Console.WriteLine(" ");


                Console.WriteLine(" ");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}