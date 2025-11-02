using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Mappings
{
    public class GroupNotificationUserMapping : Profile
    {
        public GroupNotificationUserMapping()
        {
            CreateMap<GroupNotificationUser, CreateGroupNotificationUserDto>().ReverseMap();
            CreateMap<GroupNotificationUser, GroupNotificationUserResponse>().ReverseMap();
            CreateMap<GroupNotificationUser, GroupNotificationUserDetailResponse>().ReverseMap();
        }
    }
}
