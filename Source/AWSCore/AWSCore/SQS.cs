using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWSCore
{
    using Interfaces;
    using Enums;
    public class SQS : BaseService, ISQS
    {
        private AmazonSQSClient m_client;

        public SQS(Credentials credentials) : base("AmazonSQS", ServiceTypes.SQS)
        {

            m_client = new AmazonSQSClient(credentials.PublicKey, credentials.SecretKey, RegionEndpoint.USEast2);
        }

        public async Task<AddPermissionResponse> AddPermission(string queueUrl, string label = "")
        {
            List<string> actions = new List<string>();
            actions.Add("SQS:SendMessage");
            AddPermissionRequest request = new AddPermissionRequest(queueUrl, label, new List<string>() { "252668868712" }, actions);
            AddPermissionResponse response = await m_client.AddPermissionAsync(request);
            return response;
        }

        public async Task<CreateQueueResponse> CreateQueue(string queueName)
        {
            CreateQueueRequest request = new CreateQueueRequest(queueName);
            CreateQueueResponse response = await m_client.CreateQueueAsync(request);
            return response;
        }

        public async Task<DeleteQueueResponse> DeleteQueue(string queueUrl)
        {
            DeleteQueueRequest request = new DeleteQueueRequest(queueUrl);
            DeleteQueueResponse response = await m_client.DeleteQueueAsync(request);
            return response;
        }

        public async Task<ListQueuesResponse> GetQueues()
        {
            ListQueuesRequest request = new ListQueuesRequest();
            ListQueuesResponse response = await m_client.ListQueuesAsync(request);
            return response;
        }

        public async Task<ReceiveMessageResponse> ReadMessage(string queueUrl)
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest(queueUrl);
            ReceiveMessageResponse response = await m_client.ReceiveMessageAsync(request);
            return response;
        }

        public async Task<SendMessageResponse> SendMessage(string queueUrl, string message)
        {
            SendMessageRequest request = new SendMessageRequest(queueUrl, message);
            SendMessageResponse response = await m_client.SendMessageAsync(request);
            return response;
        }

        public Amazon.Runtime.SharedInterfaces.ICoreAmazonSQS Client { get { return m_client; } }
    }
}
