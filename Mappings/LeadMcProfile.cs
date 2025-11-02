using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class LeadMcProfile: Profile
    {
        public LeadMcProfile()
        {
            CreateMap<McPersonalDto, Personal>().ReverseMap();
            CreateMap<McLeadPersonalDto, Personal>().ReverseMap();
            CreateMap<McWorkingDto, Working>().ReverseMap();
            CreateMap<McReferenceDto, Referee>().ReverseMap();
            CreateMap<McLoanDto, Loan>().ReverseMap();
            CreateMap<McAddressDto, Address>().ReverseMap();
            CreateMap<Models.Result, McResultDto>();
            CreateMap<Sale, McSaleDto>();
            CreateMap<CreateMcRequest, Customer>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => new Models.Result()))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenC))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<CreateMcStep1Request, Customer>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => new Models.Result()))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenC))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<CreateMcLeadStep1Request, Customer>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => new Models.Result()))
                .ForMember(dest => dest.GreenType, opt => opt.MapFrom(src => GreenType.GreenC))
                .ForMember(dest => dest.ProductLine, opt => opt.MapFrom(src => ProductLineConst.Lead))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.DRAFT));
            CreateMap<Customer, GetMcDetailResponse>();
            CreateMap<UpdateMcRequest, Customer>();
            CreateMap<UpdateMcStep1Request, Customer>();
            CreateMap<UpdateMcLeadStep1Request, Customer>();
            CreateMap<UpdateMcStep2Request, Customer>();
            CreateMap<UpdateMcLeadStep2Request, Customer>();
            CreateMap<UpdateMcStep3Request, Customer>();
            CreateMap<UpdateMcStep4Request, Customer>();
            CreateMap<McGroupDocumentDto, GroupDocument>().ReverseMap();
            CreateMap<McDocumentUploadDto, DocumentUpload>().ReverseMap();
            CreateMap<McUploadedMediaDto, UploadedMedia>().ReverseMap();

            CreateMap<GroupDtoModel, GroupDocument>();
            CreateMap<DocumentDtoModel, DocumentUpload>();
        }
    }
}
