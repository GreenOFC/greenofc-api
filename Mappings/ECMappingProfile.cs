using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models.EC;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class ECMappingProfile : Profile
    {
        public ECMappingProfile()
        {
            CreateMap<ECNotification, ECNotification>();
            CreateMap<ECOfferData, ECOfferDataDto>().ReverseMap();
            CreateMap<ECOfferData, ECUpdateStatusDataDto>().ReverseMap();
            CreateMap<ECOfferList, ECOfferListDto>().ReverseMap();
            CreateMap<ECOfferInsuranceList, ECOfferInsuranceListDto>().ReverseMap();
            CreateMap<ECNotification, ECUpdateStatusDto>().ReverseMap();
            CreateMap<ECDataProcessing, ECDataProsessingDetailResponse>().ReverseMap();
        }
    }
}
