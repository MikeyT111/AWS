using Amazon.SQS.Model;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface ISQS : IService
    {
        Task<SendMessageResponse> SendMessage(string queueUrl, string message);
        Task<ReceiveMessageResponse> ReadMessage(string queueUrl);
        Task<CreateQueueResponse> CreateQueue(string queueName);
        Task<DeleteQueueResponse> DeleteQueue(string queueUrl);
        Task<ListQueuesResponse> GetQueues();
        Amazon.Runtime.SharedInterfaces.ICoreAmazonSQS Client { get; }
    }
}
