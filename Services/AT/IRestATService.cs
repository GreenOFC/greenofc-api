using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.AT;
using _24hplusdotnetcore.Models.AT;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.AT
{
    public interface IRestATService
    {
        [Post("/v1/postbacks/conversions")]
        [Headers("Authorization: Token")]
        Task<ATResponseDto> PostBackConversion([Body] ATTransactionModel body);

        [Put("/v1/postbacks/conversions")]
        [Headers("Authorization: Token")]
        Task<ATResponseDto> UpdateConversion([Body] ATUpdateRequestDto body);
    }
}
