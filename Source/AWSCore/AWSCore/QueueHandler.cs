using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Interfaces;
    using Enums;

    public class QueueHandler : IHandler
    {
        private ISQS m_sqsClient;
        private bool m_shouldContinue = false;

        public QueueHandler(ISQS sqs)
        {
            m_sqsClient = sqs;
        }

        public async Task Display()
        {
            m_shouldContinue = true;

            while (m_shouldContinue)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Display Queues");
                Console.WriteLine("2: Create Queue");
                Console.WriteLine("3: Delete Queue");
                Console.WriteLine("4: Send Message");
                Console.WriteLine("5: Read Message");
                Console.WriteLine("9: Exit");

                var command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await QueueDisplayQueues();
                        break;
                    case "2":
                        await Queue_Create();
                        break;
                    case "3":
                        await Queue_Delete();
                        break;
                    case "4":
                        await Queue_SendMessage();
                        break;
                    case "5":
                        await Queue_ReadMessage();
                        break;
                    case "9":
                        m_shouldContinue = true;
                        break;
                    default:
                        Console.WriteLine("Invalid commmand");
                        break;
                }
            }
        }

        private async Task QueueDisplayQueues()
        {
            Console.WriteLine("Available Queues: ");
            var foundQueues = await m_sqsClient.GetQueues();
            foundQueues.QueueUrls.ForEach(x => Console.WriteLine("Queue URL: " + x));
            Console.WriteLine("");
        }

        private async Task Queue_Create()
        {
            Console.WriteLine("Enter new Queue name:");
            string queueName = Console.ReadLine();
            var result = await m_sqsClient.CreateQueue(queueName);
            Console.WriteLine("");
        }

        private async Task Queue_Delete()
        {
            await QueueDisplayQueues();

            Console.Write("Enter the Queue url you want to delete:");
            string queueUrl = Console.ReadLine();

            await m_sqsClient.DeleteQueue(queueUrl);
        }

        private async Task Queue_SendMessage()
        {
            await QueueDisplayQueues();
            Console.WriteLine("Enter the queue url you want to send the message to:");
            string queueUrl = Console.ReadLine();

            Console.WriteLine("Enter message you want to send to the queue:");
            string message = Console.ReadLine();

            await m_sqsClient.SendMessage(queueUrl, message);
        }

        private async Task Queue_ReadMessage()
        {
            await QueueDisplayQueues();
            Console.WriteLine("Enter the queue url you want to read the message from:");
            string queueUrl = Console.ReadLine();

            await m_sqsClient.ReadMessage(queueUrl);
        }
    }
}
