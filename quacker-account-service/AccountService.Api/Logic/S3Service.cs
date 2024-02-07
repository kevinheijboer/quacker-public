using AccountService.Api.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AccountService.Api.Logic
{
    public class S3Service : IS3Service
    {


        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            var isStaging = environment == Environments.Staging;

            string bucketName;

            if (isDevelopment)
            {
                bucketName = "quacker-bucket-dev";
            }
            else if (isStaging)
            {
                bucketName = "quacker-bucket-staging";
            }
            else
            {
                bucketName = "quacker-bucket";
            }

            using (var client = new AmazonS3Client("-", "-", RegionEndpoint.EUWest2))
            {
                if (!await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName))
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    await client.PutBucketAsync(putBucketRequest);
                }


                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = fileName,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return "https://" + bucketName + ".s3.eu-west-2.amazonaws.com/" + fileName;
                }
            }
        }
    }
}