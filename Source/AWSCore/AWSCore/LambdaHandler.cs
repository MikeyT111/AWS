using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Enums;
    using Interfaces;

    public class LambdaHandler : IHandler
    {
        private bool m_shouldContinue = false;
        private ILambda m_lambdaClient;
        private IIAM m_iamClient;
        private IS3 m_s3Client;

        public LambdaHandler(ILambda lambda, IIAM iam, IS3 s3)
        {
            m_lambdaClient = lambda;
            m_iamClient = iam;
            m_s3Client = s3;
        }

        public async Task Display()
        {
            m_shouldContinue = true;

            while (m_shouldContinue)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Display Lambdas");
                Console.WriteLine("2: Create Lambda");
                Console.WriteLine("3: Delete Lambda");
                Console.WriteLine("4: Execute Lambda");
                Console.WriteLine("9: Exit");

                var command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        await Lambda_Get();
                        break;
                    case "2":
                        await Lambda_Create();
                        break;
                    case "3":
                        await Lambda_Delete();
                        break;
                    case "4":

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

        private async Task Lambda_Get()
        {
            var response = await m_lambdaClient.GetLambdas();

            Console.WriteLine("Functions found:");
            response.Functions.ForEach(x => Console.WriteLine(x.FunctionName));
        }

        private async Task Lambda_Create()
        {
            string functionName = Utilities.DisplayMessageAndGetStringResult("Enter function name:");

            var selectedUser = await GetRole();
            if (selectedUser != null)
            {
                var selectedBucket = await GetS3Bucket();
                if (selectedBucket != null)
                {
                    var selectedKey = await GetKey(selectedBucket);
                    if (selectedKey != null)
                    {
                        string APICall = Utilities.DisplayMessageAndGetStringResult("Enter the API Call you want to use: Example: AWSCodedLamda::StepFunctionTasks.Greeting");

                        var response = await m_lambdaClient.CreateLambda(functionName, selectedUser.Arn, selectedBucket, selectedKey, APICall);

                        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK || response.HttpStatusCode == System.Net.HttpStatusCode.Created)
                        {
                            Console.WriteLine("Function Created");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create lamdba");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Key didn't exist.");
                    }
                }
                else
                {
                    Console.WriteLine("Bucket didn't exist.");
                }
            }
        }

        private async Task Lambda_Delete()
        {
            await Lambda_Get();
            string functionName = Utilities.DisplayMessageAndGetStringResult("Enter function name to delete:");
            var response = await m_lambdaClient.DeleteLambda(functionName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Deleted function");
            }
        }

        private async Task<Amazon.IdentityManagement.Model.Role> GetRole()
        {
            var roles = await m_iamClient.GetRoles();

            foreach (var role in roles.Roles)
            {
                Console.WriteLine(role.RoleName);
            }

            string roleToUser = Utilities.DisplayMessageAndGetStringResult("Enter the role from above you want.");

            return roles.Roles.FirstOrDefault(x => x.RoleName == roleToUser);
        }

        private async Task<string> GetS3Bucket()
        {
            var buckets = await m_s3Client.GetBuckets();
            Console.WriteLine("Found buckets:");
            buckets.Buckets.ForEach(X => Console.WriteLine(X.BucketName));

            string selectedBucket = Utilities.DisplayMessageAndGetStringResult("Select bucket you want to get the data from.");

            if (buckets.Buckets.Any(X => X.BucketName == selectedBucket))
            {
                return selectedBucket;
            }
            else
            {
            }
            return null;
        }

        private async Task<string> GetKey(string bucketName)
        {
            var buckets = await m_s3Client.GetObjects(bucketName);
            Console.WriteLine("Found objects:");
            buckets.S3Objects.ForEach(x => Console.WriteLine(x.Key)); //.ForEach(X => Console.WriteLine(X.BucketName));

            string selectedKey = Utilities.DisplayMessageAndGetStringResult("Select the key you want to link the lambda too");

            if (buckets.S3Objects.Any(X => X.Key == selectedKey))
            {
                return selectedKey;
            }
            else
            {
            }
            return null;
        }
    }
}
