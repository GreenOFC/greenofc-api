using _24hplusdotnetcore.ModelDtos.Otps;
using _24hplusdotnetcore.ModelDtos.Sms;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Otp
{
    public interface ISmsRestService
    {
        [Get("/Service.asmx/SendMaskedSMS")]
        Task<string> SendAsync(SmsRequest request);
    }
}
