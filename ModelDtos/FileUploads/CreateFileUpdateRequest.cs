using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos.FileUploads
{
    public class CreateFileUpdateRequest : IValidatableObject
    {
        [Required]
        [EnumDataType(typeof(RequestType))]
        public RequestType RequestType { get; set; }

        public IFormFile File { get; set; }

        public string Bas64File { get; set; }

        public string FileName { get; set; }

        [Required]
        public string BasePath { get; set; }

        public string DocumentCategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RequestType == RequestType.File && File == null)
            {
                yield return new ValidationResult(string.Format(Message.REQUIRED, nameof(File)), new string[] { nameof(File) });
            }

            if (RequestType == RequestType.Bytes && string.IsNullOrEmpty(Bas64File))
            {
                yield return new ValidationResult(string.Format(Message.REQUIRED, nameof(Bas64File)), new string[] { nameof(Bas64File) });
            }

            if (RequestType == RequestType.Bytes && string.IsNullOrEmpty(FileName))
            {
                yield return new ValidationResult(string.Format(Message.REQUIRED, nameof(FileName)), new string[] { nameof(FileName) });
            }
        }

        public async Task<byte[]> GetFileByteAsync()
        {
            if (RequestType == RequestType.File)
            {
                using var ms = new MemoryStream();
                await File.CopyToAsync(ms);
                return ms.ToArray();
            }
            else
            {
                return Convert.FromBase64String(Bas64File);
            }
        }

        public string GetFileName()
        {
            return RequestType == RequestType.File ? File.FileName : FileName;
        }
    }
}
