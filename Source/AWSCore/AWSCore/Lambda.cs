
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;

namespace AWSCore
{
    using Enums;
    using Interfaces;

    public class Lambda : BaseService, ILambda
    {
        private AmazonLambdaClient m_client;

        public Lambda(Credentials credentials) : base("AmazonLambda", ServiceTypes.Lambda)
        {
            m_client = new AmazonLambdaClient(credentials.PublicKey, credentials.SecretKey, credentials.EndPoint);
        }

        public async Task<ListFunctionsResponse> GetLambdas()
        {
            ListFunctionsRequest request = new ListFunctionsRequest();
            ListFunctionsResponse response = await m_client.ListFunctionsAsync(request);
            return response;
        }

        public async Task<CreateFunctionResponse> CreateLambda(string name, string roleARN, string s3BucketName, string s3Key, string apiCall)
        {
            CreateFunctionRequest request = new CreateFunctionRequest();
            request.FunctionName = name;
            request.Handler = apiCall; // "AWSCodedLamda::StepFunctionTasks.Greeting";
            request.Role = roleARN;
            request.Code = new FunctionCode() { S3Bucket = s3BucketName, S3Key = s3Key };
            request.Runtime = new Runtime(Runtime.Dotnetcore21);
            CreateFunctionResponse response = await m_client.CreateFunctionAsync(request);
            return response;
        }

        public async Task<DeleteFunctionResponse> DeleteLambda(string name)
        {
            DeleteFunctionRequest request = new DeleteFunctionRequest();
            request.FunctionName = name;
            DeleteFunctionResponse response = await m_client.DeleteFunctionAsync(request);
            return response;
        }

        public async Task<UpdateFunctionConfigurationResponse> UpdateLambdaConfiguration(string name)
        {
            UpdateFunctionConfigurationRequest request = new UpdateFunctionConfigurationRequest();
            request.FunctionName = name;
            UpdateFunctionConfigurationResponse response = await m_client.UpdateFunctionConfigurationAsync(request);
            return response;
        }
    }
}
