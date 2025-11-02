using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class SaleChanelConfigUserMappingProfile: Profile
    {
        public SaleChanelConfigUserMappingProfile()
        {
            CreateMap<SaleChanelConfigUser, SaleChanelConfigUserInfo>();
            CreateMap<SaleChanelConfigUserCreateRequest, SaleChanelConfigUser>();
            CreateMap<SaleChanelConfigUserUpdateRequest, SaleChanelConfigUser>();
            CreateMap<SaleChanelConfigUser, SaleChanelConfigUserCreateResponse>();
            CreateMap<SaleChanelConfigUserInfo, SaleChanelConfigUserInfoDto>();
        }
    }
}
