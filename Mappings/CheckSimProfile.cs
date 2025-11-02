using _24hplusdotnetcore.ModelDtos.CheckSims;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.ModelResponses.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class CheckSimProfile: Profile
    {
        public CheckSimProfile()
        {
            CreateMap<SendOtpRequest, CheckSim>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.RequestedMsisdn));
            CreateMap<Scoring3PRequest, CheckSim>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PrimaryPhone))
                .ForMember(dest => dest.OTP, opt => opt.MapFrom(src => src.VerificationCode));
            CreateMap<Scoring3PResponse, CheckSim>();
            CreateMap<CheckSimTransaction, CheckSimTransactionDto>();

        }
    }
}
