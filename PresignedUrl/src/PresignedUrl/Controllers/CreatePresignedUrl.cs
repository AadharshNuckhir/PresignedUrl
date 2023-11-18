using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PresignedUrl.Helper;
using PresignedUrl.Responses;

namespace PresignedUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePresignedUrl : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;

        public CreatePresignedUrl()
        {
#if DEBUG
            string key_id = ConfigHelper.AWS_ACCESS_KEY_ID;
            string key_secret = ConfigHelper.AWS_SECRET_ACCESS_KEY;
            string token = ConfigHelper.AWS_SESSION_TOKEN;

            var config = new AmazonS3Config
            {
                ServiceURL = "https://s3.amazonaws.com",
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1,
                UseHttp = true
            };

            _s3Client = new AmazonS3Client(key_id, key_secret, config);
#else
            _s3Client = new AmazonS3Client(RegionEndpoint.EUCentral1);
#endif
        }

        [HttpGet("upload/{filename}")]
        public async Task<ActionResult> GetPresignedUrl(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = Guid.NewGuid().ToString();
            }

            GetPreSignedUrlRequest presignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = ConfigHelper.S3_BUCKET_INVOICE,
                Key = filename,
                Verb = Amazon.S3.HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(ConfigHelper.TIME_TO_LIVE))
            };

            try
            {
                string presignedUrl = _s3Client.GetPreSignedURL(presignedUrlRequest);
                LambdaLogger.Log($">>> The presigned URL is : {presignedUrl}");
                return new OkObjectResult(new GetPresignedUrlResponse
                {
                    Success = true,
                    PresignedUrl = presignedUrl,
                    Filename = filename
                });
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($">>> Failed to generate presigned url for upload : {ex.Message}");
                return new BadRequestObjectResult(new GetPresignedUrlResponse
                {
                    Success = false, 
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet("download/{filename}")]
        public async Task<ActionResult> GetPresignedUrlDownload(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = Guid.NewGuid().ToString();
            }

            GetPreSignedUrlRequest presignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = ConfigHelper.S3_BUCKET_INVOICE,
                Key = filename,
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(ConfigHelper.TIME_TO_LIVE))
            };

            try
            {
                string presignedUrl = _s3Client.GetPreSignedURL(presignedUrlRequest);
                LambdaLogger.Log($">>> The presigned URL is : {presignedUrl}");
                return new OkObjectResult(new GetPresignedUrlResponse
                {
                    Success = true,
                    PresignedUrl = presignedUrl,
                    Filename = filename
                });
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($">>> Failed to generate presigned url for download : {ex.Message}");
                return new BadRequestObjectResult(new GetPresignedUrlResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

    }
}
