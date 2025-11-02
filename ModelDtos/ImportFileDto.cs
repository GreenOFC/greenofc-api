using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Models;

namespace _24hplusdotnetcore.ModelDtos
{
    public class ImportFileDto
    {
        public ImportType ImportType { get; set; }
        public string FileName { get; set; }
        public string Extensions { get; set; }
        public long FileSize { get; set; }
        public long TotalRecords { get; set; }
        public bool IsSuccess { get; set; }
        public SaleInfomation SaleInfomation { get; set; }
        public string Creator { get; set; }
    }
}
