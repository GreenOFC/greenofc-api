using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Storage
{
    public class LocalStorageService : IScopedLifetime
    {
        private readonly string BASE_PATH;
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(IWebHostEnvironment webHostEnvironment, ILogger<LocalStorageService> logger)
        {
            BASE_PATH = webHostEnvironment.ContentRootPath;
            _logger = logger;
        }

        public async Task DeleteFileAsync(string parentDirectory, string filename)
        {
            var path = $"{BASE_PATH}/FileUpload/{parentDirectory}/{filename}";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            await Task.CompletedTask;
        }

        public async Task<StorageFileResponse> GetObjectAsync(string path)
        {
            var paths = path.Split(new char[] { '/', '\\' });
            var idx = paths.ToList().IndexOf("FileUpload");
            return await Task.FromResult(new StorageFileResponse
            {
                AbsolutePath = path,
                RelativePath = string.Join("/", paths.Skip(idx + 1)),
                FileName = paths.Last(),
                Bytes = File.ReadAllBytes(path)
            });
        }

        public StorageFileResponse Upload(FileRequest fileRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(fileRequest.FileName))
                {
                    throw new ArgumentException($"File Name is required");
                }

                if (fileRequest.Bytes?.Any() != true)
                {
                    throw new ArgumentException($"File is required");
                }

                string relativeDirectory;
                switch (fileRequest.StorageFileType)
                {
                    case StorageFileType.OCRType:
                        relativeDirectory = LocalStorage.OCR_PATH;
                        break;
                    default:
                        throw new ArgumentException($"File type {fileRequest.StorageFileType} is not implemented");
                }

                if (!Directory.Exists(Path.Combine(BASE_PATH, relativeDirectory)))
                {
                    Directory.CreateDirectory(Path.Combine(BASE_PATH, relativeDirectory));
                }

                Match match = Regex.Match(fileRequest.FileName, "^(.+)\\.(.+)$");
                if (!match.Success)
                {
                    throw new ArgumentException($"File Name invalid");
                }

                string relativePath = $"{relativeDirectory}\\{match.Groups[1].Value}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{match.Groups[2].Value}";

                if (File.Exists(Path.Combine(BASE_PATH, relativePath)))
                {
                    File.Delete(Path.Combine(BASE_PATH, relativePath));
                }

                using (FileStream fs = new FileStream(Path.Combine(BASE_PATH, relativePath), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(fileRequest.Bytes, 0, fileRequest.Bytes.Length);
                }

                return new StorageFileResponse
                {
                    FileName = fileRequest.FileName,
                    RelativePath = relativePath,
                    AbsolutePath = Path.Combine(BASE_PATH, relativePath)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<StorageFileResponse> UploadFileAsync(string parentDirectory, string filename, byte[] bytes)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException($"File Name is required");
            }

            if (bytes?.Any() != true)
            {
                throw new ArgumentException($"File is required");
            }

            string relativeDirectory = $"{LocalStorage.BASE_PATH}/{parentDirectory}";
            if (!Directory.Exists(Path.Combine(BASE_PATH, relativeDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(BASE_PATH, relativeDirectory));
            }

            string relativePath = $"{relativeDirectory}/{filename}";

            if (File.Exists(Path.Combine(BASE_PATH, relativePath)))
            {
                File.Delete(Path.Combine(BASE_PATH, relativePath));
            }

            using (FileStream fs = new FileStream(Path.Combine(BASE_PATH, relativePath), FileMode.CreateNew, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            return await Task.FromResult(new StorageFileResponse
            {
                FileName = filename,
                RelativePath = relativePath,
                AbsolutePath = Path.Combine(BASE_PATH, relativePath)
            });
        }
    }
}
