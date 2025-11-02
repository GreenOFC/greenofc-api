using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.File
{
    public class FileRequestDto
    {
        [Required]
        [EnumDataType(typeof(FileType))]
        public FileType Type { get; set; }
        public string customerId { get; set; }
    }
}
