using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IRestMAFCUploadService
    {
        [Multipart]
        // [Post("/thirdparty/pushundersystem")]
        [Post("/dataentry/openapi/pushUnderSystem")]
        [Headers("Authorization: Basic")]
        Task<MAFCResponse<T>> PushToUND<T>(MAFCUploadRequestDto dto, params StreamPart[] files);
    }
}
