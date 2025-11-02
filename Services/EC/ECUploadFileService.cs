using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECUploadFileService : IScopedLifetime
    {
        private readonly ILogger<ECUploadFileService> _logger;
        private readonly ECConfig _ecConfig;

        public ECUploadFileService(ILogger<ECUploadFileService> logger, IOptions<ECConfig> ecConfig)
        {
            _logger = logger;
            _ecConfig = ecConfig.Value;
        }

        public void UploadFIle(IEnumerable<ECUploadFileDto> uploadFiles)
        {
            try
            {
                var host = _ecConfig.SFTP.Host;
                var port = _ecConfig.SFTP.Port;
                var username = _ecConfig.SFTP.Username;
                var password = _ecConfig.SFTP.Password;

                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(_ecConfig.SFTP.Username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                ConnectionInfo conInfo = new ConnectionInfo(host, port, username, keybAuth);

                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();

                    if (client.IsConnected)
                    {
                        _logger.LogInformation("connected to the client", _ecConfig.SFTP.Path);

                        var files = client.ListDirectory(_ecConfig.SFTP.Path);

                        // foreach (var file in files)
                        // {
                        //     Console.WriteLine(" - " + file.Name);
                        // }

                        foreach (var uploadFile in uploadFiles)
                        {

                            client.ChangeDirectory(_ecConfig.SFTP.Path + @"/" + uploadFile.RemoteFolder);

                            using var stream = new MemoryStream(uploadFile.Bytes);
                            // Console.WriteLine("Uploading {0} ({1:N0} bytes)", uploadFile.FileName, stream.Length);
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(stream, uploadFile.FileName);
                        }
                    }

                    else
                    {
                        _logger.LogInformation("couldn't connect client");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = _ecConfig.SFTP.Password;
                }
            }
        }
    }
}
