namespace _24hplusdotnetcore.ModelDtos.File
{
    public class FileResponse
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
