using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Interfaces;
    using Enums;

    public class S3Handler : IHandler
    {
        private bool m_shouldContinue = false;
        private IS3 m_s3;

        public S3Handler(IS3 s3)
        {
            m_s3 = s3;
        }

        public async Task Display()
        {
            m_shouldContinue = true;
            while (m_shouldContinue)
            {
                Console.WriteLine("S3 Options");
                Console.WriteLine("1: Display Buckets");
                Console.WriteLine("2: Create Buckets");
                Console.WriteLine("3: Delete Buckets");
                Console.WriteLine("4: Uploaded File");
                Console.WriteLine("5: Delete File");
                Console.WriteLine("6: Display Files");
                Console.WriteLine("7: Get File");
                Console.WriteLine("9: Exit");

                string option = Utilities.DisplayMessageAndGetStringResult("Select Option:");

                switch (option)
                {
                    case "1":
                        await DisplayBuckets();
                        break;
                    case "2":
                        await CreateBucket();
                        break;
                    case "3":
                        await DeleteBucket();
                        break;
                    case "4":
                        await UploadFile();
                        break;
                    case "5":
                        await DeleteFile();
                        break;
                    case "6":
                        await DisplayFiles();
                        break;
                    case "7":
                        await GetFile();
                        break;
                    case "9":
                        m_shouldContinue = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }
            }
        }

        private async Task DisplayBuckets()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));
            }
            else
            {
                Console.WriteLine("No buckets found.");
            }
        }

        private async Task CreateBucket()
        {
            string newBucketName = Utilities.DisplayMessageAndGetStringResult("Enter a new bucket name");
            var existingBuckets = await m_s3.GetBuckets();
            bool nameAlreadyExists = existingBuckets.Buckets.Any(x => x.BucketName == newBucketName);
            if (nameAlreadyExists)
            {
                var result = await m_s3.CreateBucket(newBucketName);
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK || result.HttpStatusCode == System.Net.HttpStatusCode.Created)
                    Console.WriteLine("Bucket Created.");
                else
                    Console.WriteLine("Failed to create bucket.");
            }
            else
            {
                Console.WriteLine("Failed to create the new bucket as the name already exists.");
            }
        }

        private async Task DeleteBucket()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));

                string bucketToDelete = Utilities.DisplayMessageAndGetStringResult("Select Bucket to delete:");
                if (response.Buckets.Any(x => x.BucketName == bucketToDelete))
                {
                    var result = await m_s3.DeleteBucket(bucketToDelete);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        Console.WriteLine("Successfully deleted bucket");
                    else
                        Console.WriteLine("Failed to delete bucket.");
                }
                else
                {
                    Console.WriteLine("Bucket doesn't exist.");
                }
            }
            else
            {
                Console.WriteLine("No buckets to delete.");
            }

        }

        private async Task UploadFile()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));

                string bucketToUse = Utilities.DisplayMessageAndGetStringResult("Enter bucket name you want to upload to.");
                if (response.Buckets.Any(x => x.BucketName == bucketToUse))
                {
                    string filepath = Utilities.DisplayMessageAndGetStringResult("Enter filepath to the zip file that you want to upload.");
                    bool exists = System.IO.File.Exists(filepath);
                    bool zipFileExtention = true;// System.IO.Path.GetExtension(filepath).ToLower() == ".zip";

                    if (exists && zipFileExtention)
                    {
                        var result = await m_s3.UploadObject(bucketToUse, filepath);
                        if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            Console.WriteLine("Succeessfully uploaded file.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to upload file.");
                        }
                    }
                    else if (!exists)
                    {
                        Console.WriteLine("File doesn't exist.");
                    }
                    else
                    {
                        Console.WriteLine("File isn't a zip folder.");
                    }
                }
                else
                {
                    Console.WriteLine("Bucket doesn't exist.");
                }
            }
        }

        private async Task DeleteFile()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));

                string bucketToUse = Utilities.DisplayMessageAndGetStringResult("Enter bucket name you want to upload to.");
                if (response.Buckets.Any(x => x.BucketName == bucketToUse))
                {
                    var objects = await m_s3.GetObjects(bucketToUse);
                    string keyToRemove = Utilities.DisplayMessageAndGetStringResult("Enter file you want to remove.");

                    if (objects.S3Objects.Any(x => x.Key == keyToRemove))
                    {
                        var result = await m_s3.DeleteObject(bucketToUse, keyToRemove);

                        if (result.HttpStatusCode == System.Net.HttpStatusCode.OK || result.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                            Console.WriteLine("Successfully deleted item.");
                        else
                            Console.WriteLine("Failed to delete item.");
                    }
                    else
                    {
                        Console.WriteLine("Key doesn't exists within bucket.");
                    }
                }
                else
                {
                    Console.WriteLine("Bucket doesn't exist.");
                }
            }
        }

        private async Task DisplayFiles()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));

                string bucketToUse = Utilities.DisplayMessageAndGetStringResult("Enter bucket name you want to get files from.");
                if (response.Buckets.Any(x => x.BucketName == bucketToUse))
                {
                    var objects = await m_s3.GetObjects(bucketToUse);

                    objects.S3Objects.ForEach(x => Console.WriteLine($"Object: {x.Key}"));
                }
                else
                {
                    Console.WriteLine("Bucket doesn't exist.");
                }
            }
        }

        private async Task GetFile()
        {
            var response = await m_s3.GetBuckets();
            if (response.Buckets.Count > 0)
            {
                Console.WriteLine("Found Buckets:");
                response.Buckets.ForEach(x => Console.WriteLine(x.BucketName));

                string bucketToUse = Utilities.DisplayMessageAndGetStringResult("Enter bucket name you want to get files from.");

                if (response.Buckets.Any(x => x.BucketName == bucketToUse))
                {
                    var objects = await m_s3.GetObjects(bucketToUse);

                    objects.S3Objects.ForEach(x => Console.WriteLine($"Object: {x.Key}"));

                    string fileToDownload = Utilities.DisplayMessageAndGetStringResult("Enter filename you want to download.");

                    if (objects.S3Objects.Any(x => x.Key == fileToDownload))
                    {
                        var file = m_s3.GetObject(bucketToUse, fileToDownload);
                    }
                }
                else
                {
                    Console.WriteLine("Bucket doesn't exist.");
                }
            }
        }

    }
}