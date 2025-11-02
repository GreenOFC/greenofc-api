using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.ModelResponses.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class MCProfile: Profile
    {
        public MCProfile()
        {
            // Check Sim
            // CreateMap<OtpResponse, SendOtpResponse>().ReverseMap();
            // CreateMap<ScoreResult, Scoring3PResponse>().ReverseMap();
            CreateMap<TransactionModel, CheckSimTransaction>();


        }
    }

}
