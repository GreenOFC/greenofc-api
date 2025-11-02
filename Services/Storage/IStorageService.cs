using _24hplusdotnetcore.ModelDtos.StorageModels;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Storage
{
    public interface IStorageService
    {
        Task<StorageFileResponse> UploadFileAsync(string parentDirectory, string filename, byte[] bytes);
        Task<StorageFileResponse> GetObjectAsync(string path);
        Task DeleteFileAsync(string parentDirectory, string filename);
    }
}
