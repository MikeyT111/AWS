using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AWSCore
{ 
    using Interfaces;
    using Enums;

    public class SNS : BaseService, ISNS
    {
        private AmazonSimpleNotificationServiceClient m_client;

        public SNS(Credentials credentials) : base("AmazonSNS", ServiceTypes.SNS)
        {
            m_client = new AmazonSimpleNotificationServiceClient(credentials.PublicKey, credentials.SecretKey, RegionEndpoint.USEast2);
        }

        public async Task<string> CreateSubscription(string arn, Amazon.Runtime.SharedInterfaces.ICoreAmazonSQS client, string queueUrl)
        {
            string response = await m_client.SubscribeQueueAsync(arn, client, queueUrl);
            return response;
        }

        public async Task<SubscribeResponse> CreateLambdaSubscription(string arn, string endpoint)
        {
            SubscribeRequest request = new SubscribeRequest(arn, "lambda", endpoint);
            SubscribeResponse response = await m_client.SubscribeAsync(request);
            return response;
        }

        public async Task<CreateTopicResponse> CreateTopic(Topic topic)
        {
            CreateTopicRequest request = new CreateTopicRequest(topic.Name);
            CreateTopicResponse response = await m_client.CreateTopicAsync(request);
            return response;
        }

        public async Task<DeleteTopicResponse> DeleteTopic(Topic topic)
        {
            DeleteTopicRequest request = new DeleteTopicRequest(topic.ARN);
            DeleteTopicResponse response = await m_client.DeleteTopicAsync(request);
            return response;
        }

        public async Task<ListTopicsResponse> GetTopics()
        {
            ListTopicsRequest request = new ListTopicsRequest();
            ListTopicsResponse response = await m_client.ListTopicsAsync(request);
            return response;
        }

        public async Task<PublishResponse> PublishMessage(Topic topic, string message, string subject = "")
        {
            PublishRequest request = new PublishRequest(topic.ARN, message, subject);
            PublishResponse response = await m_client.PublishAsync(request);
            return response;
        }

        public async Task<UnsubscribeResponse> DeleteSubscription(string subscriptionArn)
        {
            UnsubscribeRequest request = new UnsubscribeRequest(subscriptionArn);
            UnsubscribeResponse response = await m_client.UnsubscribeAsync(request);
            return response;
        }

        public async Task<ListSubscriptionsResponse> GetSubscriptions()
        {
            ListSubscriptionsRequest request = new ListSubscriptionsRequest();
            ListSubscriptionsResponse response = await m_client.ListSubscriptionsAsync(request);
            return response;
        }
    }
}
