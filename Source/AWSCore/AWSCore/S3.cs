using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Interfaces;
    using Enums;

    public class S3 : BaseService, IS3
    {
        private AmazonS3Client m_client;
        public S3(Credentials credentials) : base("S3", ServiceTypes.S3)
        {
            m_client = new AmazonS3Client(credentials.PublicKey, credentials.SecretKey, credentials.EndPoint);
        }

        public async Task<ListBucketsResponse> GetBuckets()
        {
            ListBucketsRequest request = new ListBucketsRequest();
            ListBucketsResponse response = await m_client.ListBucketsAsync(request);

            return response;
        }

        public async Task<PutBucketResponse> CreateBucket(string bucketName)
        {
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = bucketName;
            request.UseClientRegion = true;
            PutBucketResponse response = await m_client.PutBucketAsync(request);
            return response;
        }
        public async Task<DeleteBucketResponse> DeleteBucket(string bucketName)
        {
            DeleteBucketRequest request = new DeleteBucketRequest();
            request.BucketName = bucketName;
            DeleteBucketResponse response = await m_client.DeleteBucketAsync(request);
            return response;
        }

        public async Task<ListObjectsResponse> GetObjects(string bucketName)
        {
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucketName;
            ListObjectsResponse response = await m_client.ListObjectsAsync(request);
            return response;
        }

        public async Task<PutObjectResponse> UploadObject(string bucketName, string filepath)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucketName;
            request.FilePath = filepath;
            PutObjectResponse response = await m_client.PutObjectAsync(request);
            return response;
        }

        public async Task<DeleteObjectResponse> DeleteObject(string bucketName, string key)
        {
            DeleteObjectRequest request = new DeleteObjectRequest();
            request.BucketName = bucketName;
            request.Key = key;
            DeleteObjectResponse response = await m_client.DeleteObjectAsync(request);
            return response;
        }

        public async Task<GetObjectResponse> GetObject(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = bucketName;
            request.Key = key;

            GetObjectResponse response = await m_client.GetObjectAsync(request);
            Stream stream = response.ResponseStream;
            StreamReader reader = new StreamReader(stream);

            string responseBody = await reader.ReadToEndAsync();

            return response;
        }

        public async Task<string> GetTextFromObject(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = bucketName;
            request.Key = key;

            GetObjectResponse response = await m_client.GetObjectAsync(request);
            Stream stream = response.ResponseStream;
            StreamReader reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }
    }
}
