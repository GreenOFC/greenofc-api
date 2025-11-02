using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class GroupNotificationMapping : Profile
    {
        public GroupNotificationMapping()
        {
            CreateMap<GroupNotification, CreateGroupNotificationDto>().ReverseMap();
            CreateMap<GroupNotification, UpdateGroupNotificationDto>().ReverseMap();
            CreateMap<GroupNotification, GroupNotificationDetailResponse>().ReverseMap();
        }
    }
}
