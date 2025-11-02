using _24hplusdotnetcore.ModelDtos.StorageModels;
using _24hplusdotnetcore.Settings;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace _24hplusdotnetcore.Services.Storage
{
    public class S3StorageService : IStorageService, IScopedLifetime
    {
        private readonly ILogger<S3StorageService> _logger;
        private readonly IAmazonS3 _amazonS3;
        private readonly S3Config _s3Config;

        public S3StorageService(
            ILogger<S3StorageService> logger,
            IAmazonS3 amazonS3,
            IOptions<S3Config> s3Config)
        {
            _logger = logger;
            _amazonS3 = amazonS3;
            _s3Config = s3Config.Value;
        }

        public async Task<StorageFileResponse> UploadFileAsync(string parentDirectory, string filename, byte[] bytes)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new ArgumentException($"File Name is required");
                }

                if (bytes?.Any() != true)
                {
                    throw new ArgumentException($"File is required");
                }

                if (string.IsNullOrEmpty(parentDirectory))
                {
                    throw new ArgumentException($"Parent Directory is required");
                }

                if (!await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, _s3Config.BucketName))
                {
                    await _amazonS3.PutBucketAsync(new PutBucketRequest { BucketName = _s3Config.BucketName });
                }

                string key = GenerateS3Key(parentDirectory, filename);

                PutObjectRequest objectRequest = new PutObjectRequest()
                {
                    InputStream = new MemoryStream(bytes),
                    BucketName = _s3Config.BucketName,
                    Key = key,
                    CannedACL = S3CannedACL.PublicRead
                };
                objectRequest.Metadata.Add("filename", HttpUtility.UrlEncode(filename));
                await _amazonS3.PutObjectAsync(objectRequest);

                return new StorageFileResponse
                {
                    FileName = filename,
                    RelativePath = key,
                    AbsolutePath = string.Format(_s3Config.PublicUrl, _s3Config.BucketName, key)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteFileAsync(string parentDirectory, string filename)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = _s3Config.BucketName,
                    Key = GenerateS3Key(parentDirectory, filename)
                };

                await _amazonS3.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<StorageFileResponse> GetObjectAsync(string path)
        {
            try
            {
                var amazonS3Uri = new AmazonS3Uri(path);
                using GetObjectResponse response = await _amazonS3.GetObjectAsync(amazonS3Uri.Bucket, amazonS3Uri.Key);
                using MemoryStream stream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(stream);
                return new StorageFileResponse
                {
                    AbsolutePath = path,
                    RelativePath = amazonS3Uri.Key,
                    FileName = HttpUtility.UrlDecode(response.Metadata["x-amz-meta-filename"]),
                    Bytes = stream.ToArray()
                };
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private string GenerateS3Key(string parentDirectory, string filename)
        {
            string key = $"{parentDirectory}/{filename}".ToLower();
            key = Regex.Replace(key, @"[^\u0000-\u007F]+", string.Empty);
            key = Regex.Replace(key, @"\s+", "-");
            return key;
        }
    }
}
