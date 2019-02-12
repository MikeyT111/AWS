using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface ISNS : IService
    {
        Task<ListTopicsResponse> GetTopics();
        Task<CreateTopicResponse> CreateTopic(Topic topic);
        Task<DeleteTopicResponse> DeleteTopic(Topic topic);
        Task<PublishResponse> PublishMessage(Topic topic, string message, string subject = "");
        Task<string> CreateSubscription(string arn, Amazon.Runtime.SharedInterfaces.ICoreAmazonSQS client, string queueUrl);
        Task<SubscribeResponse> CreateLambdaSubscription(string arn, string endpoint);
        Task<UnsubscribeResponse> DeleteSubscription(string subscriptionArn);
        Task<ListSubscriptionsResponse> GetSubscriptions();
    }
}
