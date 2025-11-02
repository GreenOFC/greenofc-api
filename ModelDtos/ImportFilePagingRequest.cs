using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos
{
    public class ImportFilePagingRequest : PagingRequest
    {
        [Required]
        public ImportType Type { get; set; }
    }
}
