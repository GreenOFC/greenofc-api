using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelResponses.Pos;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class PosProfile : Profile
    {
        public PosProfile()
        {
            CreateMap<CreatePosDto, POS>().ReverseMap();
            CreateMap<UpdatePosDto, POS>().ReverseMap();
            CreateMap<PosDetailResponse, POS>().ReverseMap();
            CreateMap<PosInfo, PosInfoDto>();
            CreateMap<PosManager, PosManagerDto>();
            CreateMap<User, PosManagerDto>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.FullName));
        }
    }
}
