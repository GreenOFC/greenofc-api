using _24hplusdotnetcore.ModelDtos.Vps;
using _24hplusdotnetcore.ModelResponses.VPS;
using _24hplusdotnetcore.Models.VPS;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadVpsMappingProfile : Profile
    {
        public LeadVpsMappingProfile()
        {
            CreateMap<LeadVps, CreateLeadVpsDto>().ReverseMap();
            CreateMap<LeadVps, UpdateLeadVpsDto>().ReverseMap();
            CreateMap<LeadVps, LeadVpsDetailResponse>().ReverseMap();
        }
    }
}