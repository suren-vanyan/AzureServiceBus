using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Sender
{
    class SenderConsole
    {
        //ToDo: Enter a valid Serivce Bus connection string
        static string ConnectionString = "Endpoint=sb://developmentcargoo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=0Giwl/5zybNTI97H2zh1VLjtjxhtavM9/pOzXHSD5Yg=;";
        static string QueuePath = "s.demoqueue";

        static void Main(string[] args)
        {
            // Create a queue client
            var queueClient = new QueueClient(ConnectionString, QueuePath);

            // Send some messages
            for (int i = 0; i < 10; i++)
            {
                var content = $"Message: { i }";

                var message = new Message(Encoding.UTF8.GetBytes(content));
                queueClient.SendAsync(message);
                Console.WriteLine("Sent: " + i);
            }

            // Close the client
            queueClient.CloseAsync();

            Console.WriteLine("Sent messages...");
            Console.ReadLine();

        }
    }
}
