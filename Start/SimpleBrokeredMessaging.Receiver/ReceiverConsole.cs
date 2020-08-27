using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleBrokeredMessaging.Receiver
{

    class ReceiverConsole
    {
        //ToDo: Enter a valid Serivce Bus connection string
        //ToDo: Enter a valid Serivce Bus connection string
        static string ConnectionString = "Endpoint=sb://developmentcargoo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xQNQFmW9MivdcDEvx4yopBGf2Q8HeG4eSry8T+AdXmw=";
        static string QueuePath = "sv.bbctogateway";

        static IQueueClient queueClient;


        static void Main(string[] args)
        {
            Console.WriteLine("Enter 1 to exit !!!");
            while (!(Console.ReadKey().KeyChar == '0'))
            {
                Console.WriteLine();
                Console.Write("Input QueueName...  ");
                QueuePath = Console.ReadLine();
                queueClient = new QueueClient(ConnectionString, QueuePath);
                RegisterOnMessageHandlerAndReceiveMessages();
                // Close the client          
                Console.WriteLine();
            }

            queueClient.CloseAsync();
        }


        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {

            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
