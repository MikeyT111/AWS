using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Enums;
    using Interfaces;
    public class NotificationHandler : IHandler
    {
        private bool m_shouldContinue = false;
        private ISNS m_snsClient;
        private ISQS m_sqsClient;
        private ILambda m_lambdaClient;

        public NotificationHandler(ISNS sns, ISQS sqs, ILambda lambda)
        {
            m_snsClient = sns;
            m_sqsClient = sqs;
            m_lambdaClient = lambda;

        }

        public async Task Display()
        {
            m_shouldContinue = true;

            while (m_shouldContinue)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Get Topics");
                Console.WriteLine("2: Create Topic");
                Console.WriteLine("3: Delete Topic");
                Console.WriteLine("4: Publish Message");
                Console.WriteLine("5: Create a subscription");
                Console.WriteLine("6: Create a lambda subscription");
                Console.WriteLine("9: Exit");

                var command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await NotificationDisplayTopics();
                        break;
                    case "2":
                        await NotificationCreateNewTopic();
                        break;
                    case "3":
                        await NotificationDeleteTopic();
                        break;
                    case "4":
                        await NotificationPublishMessage();
                        break;
                    case "5":
                        await NotificationCreateSubscription();
                        break;
                    case "6":
                        await NotificationCreateLambdaSubscription();
                        break;
                    case "9":
                        m_shouldContinue = false;
                        break;
                    default:
                        Console.WriteLine("Invalid commmand");
                        break;
                }
            }


        }

        private async Task NotificationDisplayTopics()
        {
            Console.WriteLine("Available Topics: ");
            var foundTopics = await m_snsClient.GetTopics();
            foundTopics.Topics.ForEach(x => Console.WriteLine(x.TopicArn));
            Console.WriteLine("");
        }

        private async Task NotificationCreateNewTopic()
        {
            Console.WriteLine("Enter topic name:");
            string topicName = Console.ReadLine();

            var createTopicResponse = await m_snsClient.CreateTopic(new AWSCore.Topic(topicName));

            if (createTopicResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("Create Success");
            else
                Console.WriteLine("Failed to created new topic. Http response: " + createTopicResponse.HttpStatusCode);
        }

        private async Task NotificationDeleteTopic()
        {
            await NotificationDisplayTopics();

            Console.WriteLine("Select the ARN you want to remove.");
            var arn = Console.ReadLine();

            if (!string.IsNullOrEmpty(arn))
            {
                var deleteTopicResult = await m_snsClient.DeleteTopic(new AWSCore.Topic("", arn));

                if (deleteTopicResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    Console.WriteLine("Delete Success");
                else
                    Console.WriteLine("Failed to delete topic. Http response: " + deleteTopicResult.HttpStatusCode);
            }
        }

        private async Task NotificationPublishMessage()
        {
            await NotificationDisplayTopics();

            Console.WriteLine("Select the ARN you want to send a message to.");
            var arn = Console.ReadLine();

            if (!string.IsNullOrEmpty(arn))
            {
                Console.WriteLine("Enter Message");
                string Message = Console.ReadLine();

                Console.WriteLine("Enter Subject");
                string subject = Console.ReadLine();


                var publish = await m_snsClient.PublishMessage(new AWSCore.Topic("", arn), Message, subject);

                if (publish.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    Console.WriteLine("publish Success");
                else
                    Console.WriteLine("Failed to publish message. Http response: " + publish.HttpStatusCode);
            }
        }

        private async Task NotificationCreateSubscription()
        {
            await NotificationDisplayTopics();

            Console.WriteLine("Select the ARN you want to subscribe.");
            string arn = Console.ReadLine();

            if (!string.IsNullOrEmpty(arn))
            {
                Console.WriteLine("Available Queues: ");
                var foundQueues = await m_sqsClient.GetQueues();
                foundQueues.QueueUrls.ForEach(x => Console.WriteLine("Queue URL: " + x));
                Console.WriteLine("");

                Console.WriteLine("Select Queue URL:");
                string queueUrlToUse = Console.ReadLine();

                var subscription = await m_snsClient.CreateSubscription(arn, m_sqsClient.Client, queueUrlToUse);

                Console.WriteLine("Output: " + subscription);
            }
        }

        private async Task NotificationCreateLambdaSubscription()
        {
            await NotificationDisplayTopics();

            Console.WriteLine("Select the ARN you want to subscribe.");
            string arn = Console.ReadLine();

            if (!string.IsNullOrEmpty(arn))
            {
                Console.WriteLine("Available Functions: ");
                var foundLambdas = await m_lambdaClient.GetLambdas();

                foundLambdas.Functions.ForEach(x => Console.WriteLine("Function Name: " + x));
                Console.WriteLine("");

                Console.WriteLine("Select function name:");
                string functionNameToUse = Console.ReadLine();

                var subscription = await m_snsClient.CreateLambdaSubscription(arn, functionNameToUse);

                Console.WriteLine("Output: " + subscription);
            }
        }
    }
}
