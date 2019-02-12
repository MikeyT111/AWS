using Amazon.S3.Model;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface IS3
    {
        Task<ListBucketsResponse> GetBuckets();
        Task<PutBucketResponse> CreateBucket(string bucketName);
        Task<DeleteBucketResponse> DeleteBucket(string bucketName);
        Task<ListObjectsResponse> GetObjects(string bucketName);
        Task<PutObjectResponse> UploadObject(string bucketName, string filepath);
        Task<DeleteObjectResponse> DeleteObject(string bucketName, string key);
        Task<GetObjectResponse> GetObject(string bucketName, string key);
        Task<string> GetTextFromObject(string bucketName, string key);
    }
}
