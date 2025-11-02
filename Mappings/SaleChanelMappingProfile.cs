using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class SaleChanelMappingProfile : Profile
    {
        public SaleChanelMappingProfile()
        {
            CreateMap<SaleChanelCreateRequest, SaleChanel>();
            CreateMap<SaleChanelUpdateRequest, SaleChanel>();
            CreateMap<SaleChanel, SaleChanelInfo>();
            CreateMap<SaleChanel, SaleChanelCreateResponse>();
            CreateMap<POS, SaleChanelPosInfo>()
                .ForMember(dest => dest.CreatedDate, src => src.Ignore());
            CreateMap<SaleChanelPosInfo, SaleChanelPosInfoDto>();
            CreateMap<SaleChanel, SaleChanelDto>();
            CreateMap<SaleChanelInfo, SaleChanelDto>();
        }
    }
}
