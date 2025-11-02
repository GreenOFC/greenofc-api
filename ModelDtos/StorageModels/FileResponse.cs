using System.IO;
using System.Linq;

namespace _24hplusdotnetcore.ModelDtos.StorageModels
{
    public class StorageFileResponse
    {
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }
        public byte[] Bytes { get; set; }

        public string GetDirectory()
        {
            var paths = RelativePath.Split(new char[] { '/', '\\' });
            var idx = paths.ToList().IndexOf(FileName);
            return string.Join("/", paths.Take(idx));
        }

        public string GetExtension()
        {
            return Path.GetExtension(FileName);
        }
    }
}
