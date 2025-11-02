using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Ticket;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class TicketMappingProfile : Profile
    {
        public TicketMappingProfile()
        {
            #region Ticket
            CreateMap<CreateTicketModelDto, TicketModel>();
            CreateMap<UpdateTicketModelDto, TicketModel>();

            CreateMap<CreateCommentTicketModelDto, CommentTicketModel>();
            CreateMap<UpdateCommentTicketModelDto, CommentTicketModel>();

            CreateMap<TicketModel, GetTicketResponse>();
            CreateMap<CommentTicketModel, GetTicketCommentResponse>();

            CreateMap<DataConfigModel, DataConfigDto>();
            CreateMap<DataConfigDto, DataConfigModel>();


            
            #endregion
        }
    }
}
