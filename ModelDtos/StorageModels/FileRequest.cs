using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.StorageModels
{
    public class FileRequest
    {
        public StorageFileType StorageFileType { get; set; }
        public string FileName { get; set; }
        public byte[] Bytes { get; set; }
    }
}
