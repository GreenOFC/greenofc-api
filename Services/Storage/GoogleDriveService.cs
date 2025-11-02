using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Storage
{
    public class GoogleDriveService : IStorageService
    {
        private readonly string BasePath = "FileUpload";

        public GoogleDriveService()
        {
        }

        public StorageFileResponse Upload(FileRequest fileRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<StorageFileResponse> UploadFileAsync(string parentDirectory, string filename, byte[] bytes)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException($"fileName");

            if (bytes?.Any() != true)
                throw new ArgumentException($"bytes");

            using var service = GetDriveService("credentials.json", "user", new string[] { DriveService.Scope.DriveFile });
            var baseFolder = List(service, new FilesListOptionalParms
            {
                PageSize = 1,
                Q = $"mimeType = 'application/vnd.google-apps.folder' and parents='root'  and name = '{BasePath}' and trashed = false"
            }).Files.FirstOrDefault();
            if (baseFolder == null)
            {
                baseFolder = Create(service, new Google.Apis.Drive.v3.Data.File
                {
                    Name = BasePath,
                    MimeType = "application/vnd.google-apps.folder",
                    Parents = new List<string> { "root" }
                });
            }

            var parentFolder = List(service, new FilesListOptionalParms
            {
                PageSize = 1,
                Q = $"mimeType = 'application/vnd.google-apps.folder' and parents='{baseFolder.Id}'  and name = '{parentDirectory}' and trashed = false"
            }).Files.FirstOrDefault();
            if (parentFolder == null)
            {
                parentFolder = Create(service, new Google.Apis.Drive.v3.Data.File
                {
                    Name = parentDirectory,
                    MimeType = "application/vnd.google-apps.folder",
                    Parents = new List<string> { baseFolder.Id }
                });
            }

            using MemoryStream stream = new MemoryStream(bytes);
            var file = Upload(service, new Google.Apis.Drive.v3.Data.File
            {
                Name = filename,
                Parents = new List<string> { parentFolder.Id }
            }, stream, GetMimeType(filename));

            return await Task.FromResult(new StorageFileResponse
            {
                FileId = file?.Id,
                FileName = filename
            });
        }


        private string GetMimeType(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            var mimeType = MimeTypeMap.GetMimeType(ext);
            return mimeType;
        }

        private Google.Apis.Drive.v3.Data.File Create(DriveService service, Google.Apis.Drive.v3.Data.File body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");

                // Building the initial request.
                var request = service.Files.Create(body);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Create failed.", ex);
            }
        }

        private Google.Apis.Drive.v3.Data.File Upload(DriveService service, Google.Apis.Drive.v3.Data.File body, Stream stream, string contentType)
        {
            // Initial validation.
            if (service == null)
                throw new ArgumentNullException("service");
            if (body == null)
                throw new ArgumentNullException("body");
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            // Building the initial request.
            var request = service.Files.Create(body, stream, contentType);

            // Requesting data.
            request.Upload();

            return request.ResponseBody;
        }

        private class FilesListOptionalParms
        {
            /// A comma-separated list of sort keys. Valid keys are 'createdTime', 'folder', 'modifiedByMeTime', 'modifiedTime', 'name', 'name_natural', 'quotaBytesUsed', 'recency', 'sharedWithMeTime', 'starred', and 'viewedByMeTime'. Each key sorts ascending by default, but may be reversed with the 'desc' modifier. Example usage: ?orderBy=folder,modifiedTime desc,name. Please note that there is a current limitation for users with approximately one million files in which the requested sort order is ignored.
            public string OrderBy { get; set; }
            /// The maximum number of files to return per page. Partial or empty result pages are possible even before the end of the files list has been reached.
            public int? PageSize { get; set; }
            /// A query for filtering the file results. See the "Search for Files" guide for supported syntax.
            public string Q { get; set; }
        }

        private FileList List(DriveService service, FilesListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                // Building the initial request.
                var request = service.Files.List();

                // Applying optional parameters to the request.                
                request = (FilesResource.ListRequest)ObjectHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.List failed.", ex);
            }
        }

        private DriveService GetDriveService(string clientSecretJson, string userName, string[] scopes)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                var cred = GetUserCredential(clientSecretJson, userName, scopes);
                return GetService(cred);

            }
            catch (Exception ex)
            {
                throw new Exception("Get Drive service failed.", ex);
            }
        }

        private UserCredential GetUserCredential(string clientSecretJson, string userName, string[] scopes)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them               
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                    // Requesting Authentication or loading previously stored authentication for userName
                    var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                             scopes,
                                                                             userName,
                                                                             CancellationToken.None,
                                                                             new FileDataStore(credPath, true)).Result;

                    credential.GetAccessTokenForRequestAsync();
                    return credential;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get user credentials failed.", ex);
            }
        }

        private DriveService GetService(UserCredential credential)
        {
            try
            {
                if (credential == null)
                    throw new ArgumentNullException("credential");

                // Create Drive API service.
                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive Oauth2 Authentication"
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Get Drive service failed.", ex);
            }
        }

        public Task DeleteFileAsync(string parentDirectory, string filename)
        {
            throw new NotImplementedException();
        }

        public Task<StorageFileResponse> GetObjectAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
