using Amazon.Lambda.Model;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface ILambda
    {
        Task<ListFunctionsResponse> GetLambdas();
        Task<CreateFunctionResponse> CreateLambda(string name, string roleARN, string s3BucketName, string s3Key, string apiCall);
        Task<DeleteFunctionResponse> DeleteLambda(string name);
    }
}
